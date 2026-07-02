namespace Backend.Api.DTOs.Auth;

public class AuthResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
