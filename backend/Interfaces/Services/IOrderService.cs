using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Orders;

namespace Backend.Api.Interfaces.Services;

public interface IOrderService
{
    Task<ServiceResult<List<OrderHistoryDto>>> GetByUserIdAsync(string userId);
}
