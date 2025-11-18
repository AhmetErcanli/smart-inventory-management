using SmartInventory.Domain.Enums;

namespace SmartInventory.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public int ProductId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public int? UserId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual User? User { get; set; }
}

