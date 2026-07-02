using Backend.Api.Data;
using Backend.Api.Interfaces.Repositories;
using Backend.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sneaker>> GetAllAsync()
    {
        return await _context.Sneakers
            .Include(s => s.IdCategoriaNavigation)
            .Include(s => s.Inventarios)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    public async Task<Sneaker?> GetByIdAsync(int id)
    {
        return await _context.Sneakers
            .Include(s => s.IdCategoriaNavigation)
            .Include(s => s.Inventarios)
                .ThenInclude(i => i.IdColorNavigation)
            .Include(s => s.Inventarios)
                .ThenInclude(i => i.IdTallaNavigation)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Categoria>> GetCategoriesAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }
}
