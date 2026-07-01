using Backend.Api.DTOs.Auth;
using Backend.Api.DTOs.Common;

namespace Backend.Api.Interfaces.Services;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequestDto request);
}
