namespace SmartInventory.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string WarehouseName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int? Capacity { get; set; }

    // Navigation properties
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

