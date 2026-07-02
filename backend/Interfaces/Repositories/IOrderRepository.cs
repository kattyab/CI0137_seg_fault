using Backend.Api.Models;

namespace Backend.Api.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<List<Ordene>> GetByUserIdAsync(string userId);
}
