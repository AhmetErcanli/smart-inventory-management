namespace SmartInventory.Domain.Entities;

public class LowStockAlert : BaseEntity
{
    public int ProductId { get; set; }
    public int CurrentQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public bool IsResolved { get; set; } = false;
    public DateTime? ResolvedAt { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
}

