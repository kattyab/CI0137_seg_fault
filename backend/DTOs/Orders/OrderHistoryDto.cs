namespace Backend.Api.DTOs.Orders;

public class OrderHistoryDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<OrderHistoryItemDto> Items { get; set; } = new();
}

public class OrderHistoryItemDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Talla { get; set; } = string.Empty;
    public string? Color { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnit { get; set; }
    public string? ImagenUrl { get; set; }
}
