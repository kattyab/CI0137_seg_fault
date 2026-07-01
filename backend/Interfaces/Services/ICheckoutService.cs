using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Orders;

namespace Backend.Api.Interfaces.Services;

public interface ICheckoutService
{
    Task<ServiceResult<CheckoutResponseDto>> CheckoutAsync(CheckoutRequestDto request);
}
