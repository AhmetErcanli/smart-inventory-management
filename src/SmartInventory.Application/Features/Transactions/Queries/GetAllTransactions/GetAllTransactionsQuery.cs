using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Features.Transactions.Queries.GetAllTransactions;

public class GetAllTransactionsQuery : IRequest<ApiResponse<PagedResultDto<InventoryTransactionDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? ProductId { get; set; }
}

