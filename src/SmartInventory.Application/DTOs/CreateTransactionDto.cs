using SmartInventory.Domain.Enums;

namespace SmartInventory.Application.DTOs;

public class CreateTransactionDto
{
    public int ProductId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}

