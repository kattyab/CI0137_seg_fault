namespace Backend.Api.DTOs.Orders;

public class CheckoutResponseDto
{
    public string OrderId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string? AuthorizationCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
