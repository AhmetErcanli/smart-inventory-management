using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Interfaces;
using System.Linq.Expressions;

namespace SmartInventory.Application.Features.Transactions.Queries.GetAllTransactions;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, ApiResponse<PagedResultDto<InventoryTransactionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllTransactionsQueryHandler> _logger;

    public GetAllTransactionsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetAllTransactionsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResultDto<InventoryTransactionDto>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Expression<Func<Domain.Entities.InventoryTransaction, bool>>? predicate = null;
            if (request.ProductId.HasValue)
                predicate = t => t.ProductId == request.ProductId.Value;

            var allTransactions = await _unitOfWork.InventoryTransactions.FindAsync(
                predicate ?? (t => true), cancellationToken);

            var transactionsList = allTransactions.OrderByDescending(t => t.CreatedAt).ToList();

            var totalCount = transactionsList.Count;
            var pagedTransactions = transactionsList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var transactionDtos = _mapper.Map<List<InventoryTransactionDto>>(pagedTransactions);

            foreach (var dto in transactionDtos)
            {
                var transaction = pagedTransactions.First(t => t.Id == dto.Id);
                if (transaction.Product != null)
                    dto.ProductName = transaction.Product.Name;
                if (transaction.User != null)
                    dto.Username = transaction.User.Username;
            }

            var result = new PagedResultDto<InventoryTransactionDto>
            {
                Items = transactionDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return ApiResponse<PagedResultDto<InventoryTransactionDto>>.SuccessResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions");
            return ApiResponse<PagedResultDto<InventoryTransactionDto>>.ErrorResponse("An error occurred while retrieving transactions.");
        }
    }
}

