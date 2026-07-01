using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Products;

namespace Backend.Api.Interfaces.Services;

public interface IProductService
{
    Task<ServiceResult<List<ProductListItemDto>>> GetAllAsync();
    Task<ServiceResult<ProductDetailDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<CategoryDto>>> GetCategoriesAsync();
}
