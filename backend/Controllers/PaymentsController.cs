using Backend.Api.DTOs.Orders;
using Backend.Api.DTOs.Payments;
using Backend.Api.Interfaces.Services;
using Backend.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;
    private readonly PaymentValidationService _paymentValidationService;

    public PaymentsController(
        ICheckoutService checkoutService,
        PaymentValidationService paymentValidationService)
    {
        _checkoutService = checkoutService;
        _paymentValidationService = paymentValidationService;
    }

    [HttpPost("validate-card")]
    public IActionResult ValidateCard(CardValidationRequestDto request)
    {
        var result = _paymentValidationService.ValidateCardNumber(request.CardNumber);

        if (!result.Success)
            return Ok(new CardValidationResponseDto { Valid = false, Error = result.Error });

        return Ok(new CardValidationResponseDto
        {
            Valid = true,
            Brand = result.Brand,
            IssuerBank = result.IssuerBank,
            Last4 = result.Last4
        });
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CheckoutRequestDto request)
    {
        var result = await _checkoutService.CheckoutAsync(request);

        if (!result.Success)
        {
            if (result.Data != null)
                return BadRequest(result.Data);

            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }
}
