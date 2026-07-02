using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Orders;
using Backend.Api.Interfaces.Repositories;
using Backend.Api.Interfaces.Services;

namespace Backend.Api.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ServiceResult<List<OrderHistoryDto>>> GetByUserIdAsync(string userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        // Solo compras concretadas: las órdenes rechazadas no tienen detalle
        var response = orders
            .Where(o => o.Estado == "Completado")
            .Select(o => new OrderHistoryDto
            {
                Id = o.Id,
                Fecha = o.Fecha,
                Estado = o.Estado,
                Total = o.Total,
                Items = o.DetalleOrdens.Select(d => new OrderHistoryItemDto
                {
                    Nombre = d.IdInventarioNavigation.IdSneakerNavigation.Nombre,
                    Talla = d.IdInventarioNavigation.IdTallaNavigation.Nombre,
                    Color = d.IdInventarioNavigation.IdColorNavigation?.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnit = d.PrecioUnit,
                    ImagenUrl = d.IdInventarioNavigation.ImagenUrl
                }).ToList()
            }).ToList();

        return ServiceResult<List<OrderHistoryDto>>.Ok(response);
    }
}
