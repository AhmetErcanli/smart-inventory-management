namespace SmartInventory.Application.DTOs;

public class UpdateWarehouseDto
{
    public int Id { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int? Capacity { get; set; }
}

