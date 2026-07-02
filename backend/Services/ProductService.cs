using Backend.Api.DTOs.Common;
using Backend.Api.DTOs.Products;
using Backend.Api.Interfaces.Repositories;
using Backend.Api.Interfaces.Services;

namespace Backend.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ServiceResult<List<ProductListItemDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        var response = products.Select(p => new ProductListItemDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Categoria = p.IdCategoriaNavigation.Nombre,
            Precio = p.Precio,
            ImagenUrl = p.Inventarios
                .OrderBy(i => i.Id)
                .Select(i => i.ImagenUrl)
                .FirstOrDefault(u => u != null)
        }).ToList();

        return ServiceResult<List<ProductListItemDto>>.Ok(response);
    }

    public async Task<ServiceResult<ProductDetailDto>> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return ServiceResult<ProductDetailDto>.Fail("El producto no existe.");

        var response = new ProductDetailDto
        {
            Id = product.Id,
            Nombre = product.Nombre,
            Categoria = product.IdCategoriaNavigation.Nombre,
            Precio = product.Precio,
            Descripcion = product.Descripcion,
            ImagenUrl = product.Inventarios
                .OrderBy(i => i.Id)
                .Select(i => i.ImagenUrl)
                .FirstOrDefault(u => u != null),
            Inventario = product.Inventarios.Select(i => new InventoryOptionDto
            {
                InventoryId = i.Id,
                Color = i.IdColorNavigation?.Nombre,
                Talla = i.IdTallaNavigation.Nombre,
                Stock = i.Stock,
                ImagenUrl = i.ImagenUrl
            }).ToList()
        };

        return ServiceResult<ProductDetailDto>.Ok(response);
    }

    public async Task<ServiceResult<List<CategoryDto>>> GetCategoriesAsync()
    {
        var categories = await _productRepository.GetCategoriesAsync();

        var response = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Nombre = c.Nombre
        }).ToList();

        return ServiceResult<List<CategoryDto>>.Ok(response);
    }
}
