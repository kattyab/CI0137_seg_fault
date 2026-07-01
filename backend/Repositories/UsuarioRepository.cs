using Backend.Api.Data;
using Backend.Api.Interfaces.Repositories;
using Backend.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetByIdAsync(string id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }
}
