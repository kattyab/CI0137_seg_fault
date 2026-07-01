namespace Backend.Api.DTOs.Payments;

public class PaymentRequestDto
{
    public string CardholderName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public int ExpMonth { get; set; }
    public int ExpYear { get; set; }
    public string Cvc { get; set; } = string.Empty;
}
