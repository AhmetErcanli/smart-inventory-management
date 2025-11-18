using SmartInventory.Domain.Enums;

namespace SmartInventory.Application.DTOs;

public class InventoryTransactionDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public TransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public DateTime CreatedAt { get; set; }
}

