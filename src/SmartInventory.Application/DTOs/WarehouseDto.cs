namespace SmartInventory.Application.DTOs;

public class WarehouseDto
{
    public int Id { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int? Capacity { get; set; }
    public DateTime CreatedAt { get; set; }
}

