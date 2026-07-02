namespace Backend.Api.DTOs.Products;

public class ProductDetailDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public List<InventoryOptionDto> Inventario { get; set; } = new();
}

public class InventoryOptionDto
{
    public int InventoryId { get; set; }
    public string? Color { get; set; }
    public string Talla { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string? ImagenUrl { get; set; }
}
