using Backend.Api.Data;
using Backend.Api.Interfaces.Repositories;
using Backend.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ordene>> GetByUserIdAsync(string userId)
    {
        return await _context.Ordenes
            .Where(o => o.IdUsuario == userId)
            .Include(o => o.DetalleOrdens)
                .ThenInclude(d => d.IdInventarioNavigation)
                    .ThenInclude(i => i.IdSneakerNavigation)
            .Include(o => o.DetalleOrdens)
                .ThenInclude(d => d.IdInventarioNavigation)
                    .ThenInclude(i => i.IdColorNavigation)
            .Include(o => o.DetalleOrdens)
                .ThenInclude(d => d.IdInventarioNavigation)
                    .ThenInclude(i => i.IdTallaNavigation)
            .OrderByDescending(o => o.Fecha)
            .ToListAsync();
    }
}
