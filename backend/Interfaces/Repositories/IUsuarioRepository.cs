using Backend.Api.Models;

namespace Backend.Api.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(string id);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(Usuario usuario);
}
