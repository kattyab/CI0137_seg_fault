using Backend.Api.Data;
using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Orders;
using Backend.Api.Interfaces.Services;
using Backend.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Services;

public class CheckoutService : ICheckoutService
{
    private readonly AppDbContext _context;
    private readonly PaymentValidationService _paymentValidationService;
    private readonly PaymentGatewayService _paymentGatewayService;

    public CheckoutService(
        AppDbContext context,
        PaymentValidationService paymentValidationService,
        PaymentGatewayService paymentGatewayService)
    {
        _context = context;
        _paymentValidationService = paymentValidationService;
        _paymentGatewayService = paymentGatewayService;
    }

    public async Task<ServiceResult<CheckoutResponseDto>> CheckoutAsync(CheckoutRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return ServiceResult<CheckoutResponseDto>.Fail("El usuario es obligatorio.");

        if (request.Items.Count == 0)
            return ServiceResult<CheckoutResponseDto>.Fail("El carrito está vacío.");

        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
            return ServiceResult<CheckoutResponseDto>.Fail("El usuario no existe.");

        var paymentValidation = _paymentValidationService.Validate(request.Payment);

        if (!paymentValidation.Success)
            return ServiceResult<CheckoutResponseDto>.Fail(paymentValidation.Error!);

        decimal total = 0;

        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0)
                return ServiceResult<CheckoutResponseDto>.Fail("La cantidad debe ser mayor a cero.");

            var inventory = await _context.Inventarios
                .Include(i => i.IdSneakerNavigation)
                .FirstOrDefaultAsync(i => i.Id == item.InventoryId);

            if (inventory == null)
                return ServiceResult<CheckoutResponseDto>.Fail($"No existe el inventario {item.InventoryId}.");

            if (inventory.Stock < item.Quantity)
                return ServiceResult<CheckoutResponseDto>.Fail($"No hay suficiente stock para {inventory.IdSneakerNavigation.Nombre}.");

            total += inventory.IdSneakerNavigation.Precio * item.Quantity;
        }

        var gatewayResult = _paymentGatewayService.Authorize(
            request.Payment.CardNumber,
            total
        );

        using var transaction = await _context.Database.BeginTransactionAsync();

        string orderId = $"ord_{Guid.NewGuid().ToString("N")[..20]}";
        string paymentId = $"pay_{Guid.NewGuid().ToString("N")[..20]}";

        var order = new Ordene
        {
            Id = orderId,
            IdUsuario = request.UserId,
            Fecha = DateTime.UtcNow,
            Estado = gatewayResult.IsApproved ? "Completado" : "Cancelado",
            Total = total
        };

        _context.Ordenes.Add(order);

        if (gatewayResult.IsApproved)
        {
            foreach (var item in request.Items)
            {
                var inventory = await _context.Inventarios
                    .Include(i => i.IdSneakerNavigation)
                    .FirstAsync(i => i.Id == item.InventoryId);

                inventory.Stock -= item.Quantity;

                var detail = new DetalleOrden
                {
                    IdOrden = orderId,
                    IdInventario = item.InventoryId,
                    Cantidad = item.Quantity,
                    PrecioUnit = inventory.IdSneakerNavigation.Precio
                };

                _context.DetalleOrdens.Add(detail);
            }
        }

        int expYear = request.Payment.ExpYear < 100
            ? request.Payment.ExpYear + 2000
            : request.Payment.ExpYear;

        var payment = new TransaccionesPago
        {
            Id = paymentId,
            IdOrden = orderId,
            NombreTarjeta = request.Payment.CardholderName.Trim(),
            BinTarjeta = paymentValidation.Bin,
            Ultimos4 = paymentValidation.Last4,
            Marca = paymentValidation.Brand,
            ExpMes = request.Payment.ExpMonth,
            ExpAnio = expYear,
            Monto = total,
            Estado = gatewayResult.IsApproved ? "Aprobada" : "Rechazada",
            CodigoAutorizacion = gatewayResult.AuthorizationCode,
            MotivoRechazo = gatewayResult.RejectionReason,
            CreatedDate = DateTime.UtcNow
        };

        _context.TransaccionesPagos.Add(payment);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var response = new CheckoutResponseDto
        {
            OrderId = orderId,
            PaymentId = paymentId,
            Status = payment.Estado,
            Total = total,
            AuthorizationCode = gatewayResult.AuthorizationCode,
            Message = gatewayResult.IsApproved
                ? "Pago aprobado correctamente."
                : gatewayResult.RejectionReason ?? "Pago rechazado."
        };

        if (!gatewayResult.IsApproved)
            return ServiceResult<CheckoutResponseDto>.Fail(response.Message, response);

        return ServiceResult<CheckoutResponseDto>.Ok(response);
    }
}
