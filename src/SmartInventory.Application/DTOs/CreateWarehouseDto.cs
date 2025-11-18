namespace SmartInventory.Application.DTOs;

public class CreateWarehouseDto
{
    public string WarehouseName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int? Capacity { get; set; }
}

