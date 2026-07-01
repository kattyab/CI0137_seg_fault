using Backend.Api.DTOs.Orders;
using Backend.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public PaymentsController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
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
