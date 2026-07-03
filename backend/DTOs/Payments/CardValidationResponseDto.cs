namespace Backend.Api.DTOs.Payments;

public class CardValidationResponseDto
{
    public bool Valid { get; set; }
    public string? Error { get; set; }
    public string? Brand { get; set; }
    public string? IssuerBank { get; set; }
    public string? Last4 { get; set; }
}
