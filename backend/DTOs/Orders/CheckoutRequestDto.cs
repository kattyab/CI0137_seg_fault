using Backend.Api.DTOs.Payments;

namespace Backend.Api.DTOs.Orders;

public class CheckoutRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public List<CheckoutItemDto> Items { get; set; } = new();
    public PaymentRequestDto Payment { get; set; } = new();
}

public class CheckoutItemDto
{
    public int InventoryId { get; set; }
    public int Quantity { get; set; }
}
