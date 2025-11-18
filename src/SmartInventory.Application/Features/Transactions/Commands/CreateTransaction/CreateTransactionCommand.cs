using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<ApiResponse<InventoryTransactionDto>>
{
    public CreateTransactionDto TransactionDto { get; set; } = null!;
    public int UserId { get; set; }
}

