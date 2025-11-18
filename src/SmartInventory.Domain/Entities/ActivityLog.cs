namespace SmartInventory.Domain.Entities;

public class ActivityLog : BaseEntity
{
    public int? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? Details { get; set; }
    public string? IpAddress { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
}

