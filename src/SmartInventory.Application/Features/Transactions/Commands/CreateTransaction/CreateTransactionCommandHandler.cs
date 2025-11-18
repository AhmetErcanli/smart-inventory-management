using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Enums;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ApiResponse<InventoryTransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTransactionCommandHandler> _logger;

    public CreateTransactionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateTransactionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<InventoryTransactionDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.TransactionDto.ProductId, cancellationToken);
            if (product == null)
                return ApiResponse<InventoryTransactionDto>.ErrorResponse("Product not found.");

            // Validate stock out
            if (request.TransactionDto.TransactionType == TransactionType.StockOut)
            {
                if (product.Quantity < request.TransactionDto.Quantity)
                    return ApiResponse<InventoryTransactionDto>.ErrorResponse(
                        $"Insufficient stock. Available: {product.Quantity}, Requested: {request.TransactionDto.Quantity}");
            }

            // Create transaction
            var transaction = new InventoryTransaction
            {
                ProductId = request.TransactionDto.ProductId,
                TransactionType = request.TransactionDto.TransactionType,
                Quantity = request.TransactionDto.Quantity,
                Notes = request.TransactionDto.Notes,
                UserId = request.UserId
            };

            // Update product quantity
            if (request.TransactionDto.TransactionType == TransactionType.StockIn)
                product.Quantity += request.TransactionDto.Quantity;
            else
                product.Quantity -= request.TransactionDto.Quantity;

            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.InventoryTransactions.AddAsync(transaction, cancellationToken);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var transactionDto = _mapper.Map<InventoryTransactionDto>(transaction);
            transactionDto.ProductName = product.Name;
            if (transaction.User != null)
                transactionDto.Username = transaction.User.Username;

            _logger.LogInformation("Transaction created: {TransactionId} - Type: {Type}, Quantity: {Quantity}",
                transaction.Id, transaction.TransactionType, transaction.Quantity);

            return ApiResponse<InventoryTransactionDto>.SuccessResponse(transactionDto, "Transaction created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction");
            return ApiResponse<InventoryTransactionDto>.ErrorResponse("An error occurred while creating the transaction.");
        }
    }
}

