using Backend.Api.DTOs.Auth;
using Backend.Api.DTOs.Common;
using Backend.Api.Interfaces.Repositories;
using Backend.Api.Interfaces.Services;
using Backend.Api.Models;

namespace Backend.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly PasswordService _passwordService;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        PasswordService passwordService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
    }

    public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return ServiceResult<AuthResponseDto>.Fail("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Email))
            return ServiceResult<AuthResponseDto>.Fail("El correo es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Telefono))
            return ServiceResult<AuthResponseDto>.Fail("El teléfono es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return ServiceResult<AuthResponseDto>.Fail("La contraseña es obligatoria.");

        string email = request.Email.Trim().ToLower();

        bool emailExists = await _usuarioRepository.EmailExistsAsync(email);

        if (emailExists)
            return ServiceResult<AuthResponseDto>.Fail("El correo ya está registrado.");

        string salt = _passwordService.GenerateSalt();
        string hash = _passwordService.HashPassword(request.Password, salt);

        var usuario = new Usuario
        {
            Id = $"user_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            Nombre = request.Nombre.Trim(),
            Email = email,
            Telefono = request.Telefono.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            CreatedDate = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario);

        var response = new AuthResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            Token = $"session_{Guid.NewGuid()}"
        };

        return ServiceResult<AuthResponseDto>.Ok(response);
    }

    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return ServiceResult<AuthResponseDto>.Fail("El correo es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return ServiceResult<AuthResponseDto>.Fail("La contraseña es obligatoria.");

        string email = request.Email.Trim().ToLower();

        var usuario = await _usuarioRepository.GetByEmailAsync(email);

        if (usuario == null)
            return ServiceResult<AuthResponseDto>.Fail("Correo o contraseña incorrectos.");

        bool isValidPassword = _passwordService.VerifyPassword(
            request.Password,
            usuario.PasswordSalt,
            usuario.PasswordHash
        );

        if (!isValidPassword)
            return ServiceResult<AuthResponseDto>.Fail("Correo o contraseña incorrectos.");

        var response = new AuthResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            Token = $"session_{Guid.NewGuid()}"
        };

        return ServiceResult<AuthResponseDto>.Ok(response);
    }
}
