namespace SmartInventory.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public int WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public int Quantity { get; set; }
    public int MinStockLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

