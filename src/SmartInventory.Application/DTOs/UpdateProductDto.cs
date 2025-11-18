namespace SmartInventory.Application.DTOs;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
    public int MinStockLevel { get; set; }
}

