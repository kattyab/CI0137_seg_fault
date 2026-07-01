using Backend.Api.Models;

namespace Backend.Api.Interfaces.Repositories;

public interface IProductRepository
{
    Task<List<Sneaker>> GetAllAsync();
    Task<Sneaker?> GetByIdAsync(int id);
    Task<List<Categoria>> GetCategoriesAsync();
}
