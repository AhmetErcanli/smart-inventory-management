using MediatR;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Enums;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Features.Dashboard.Queries.GetDashboardData;

public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, ApiResponse<DashboardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetDashboardDataQueryHandler> _logger;

    public GetDashboardDataQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetDashboardDataQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<DashboardDto>> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = (await _unitOfWork.Products.GetAllAsync(cancellationToken)).ToList();
            var transactions = (await _unitOfWork.InventoryTransactions.GetAllAsync(cancellationToken)).ToList();
            var categories = (await _unitOfWork.Categories.GetAllAsync(cancellationToken)).ToList();

            var dashboard = new DashboardDto
            {
                TotalProducts = products.Count,
                TotalStockValue = products.Sum(p => p.Quantity * 100), // Assuming average price of 100 per unit
                TopLowStockProducts = products
                    .Where(p => p.Quantity < p.MinStockLevel)
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .Select(p => new LowStockProductDto
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        SKU = p.SKU,
                        CurrentQuantity = p.Quantity,
                        MinStockLevel = p.MinStockLevel
                    })
                    .ToList(),
                MonthlyTransactions = transactions
                    .GroupBy(t => new { t.CreatedAt.Year, t.CreatedAt.Month })
                    .Select(g => new MonthlyTransactionDto
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        StockIn = g.Where(t => t.TransactionType == TransactionType.StockIn).Sum(t => t.Quantity),
                        StockOut = g.Where(t => t.TransactionType == TransactionType.StockOut).Sum(t => t.Quantity)
                    })
                    .OrderByDescending(m => m.Month)
                    .Take(12)
                    .ToList(),
                CategoryDistribution = categories
                    .Select(c => new CategoryDistributionDto
                    {
                        CategoryName = c.Name,
                        ProductCount = products.Count(p => p.CategoryId == c.Id),
                        TotalQuantity = products.Where(p => p.CategoryId == c.Id).Sum(p => p.Quantity)
                    })
                    .ToList()
            };

            return ApiResponse<DashboardDto>.SuccessResponse(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard data");
            return ApiResponse<DashboardDto>.ErrorResponse("An error occurred while retrieving dashboard data.");
        }
    }
}

