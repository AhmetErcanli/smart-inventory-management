using Hangfire;
using Microsoft.Extensions.Logging;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Infrastructure.Jobs;

public class LowStockAlertJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<LowStockAlertJob> _logger;

    public LowStockAlertJob(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<LowStockAlertJob> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task CheckLowStockProducts()
    {
        try
        {
            _logger.LogInformation("Low stock alert job started at {Time}", DateTime.UtcNow);

            var products = await _unitOfWork.Products.GetAllAsync();
            var lowStockProducts = products.Where(p => p.Quantity < p.MinStockLevel && p.Quantity >= 0).ToList();

            foreach (var product in lowStockProducts)
            {
                // Check if alert already exists and is not resolved
                var existingAlert = await _unitOfWork.LowStockAlerts.FirstOrDefaultAsync(
                    a => a.ProductId == product.Id && !a.IsResolved);

                if (existingAlert == null)
                {
                    // Create new alert
                    var alert = new LowStockAlert
                    {
                        ProductId = product.Id,
                        CurrentQuantity = product.Quantity,
                        MinStockLevel = product.MinStockLevel,
                        IsResolved = false
                    };

                    await _unitOfWork.LowStockAlerts.AddAsync(alert);

                    // Send email notification (if supplier email exists)
                    if (product.Supplier != null && !string.IsNullOrEmpty(product.Supplier.Email))
                    {
                        await _emailService.SendLowStockAlertAsync(
                            product.Supplier.Email,
                            product.Name,
                            product.Quantity,
                            product.MinStockLevel);
                    }

                    _logger.LogWarning(
                        "Low stock alert created for product: {ProductName} (ID: {ProductId}), Current: {Quantity}, Min: {MinStock}",
                        product.Name, product.Id, product.Quantity, product.MinStockLevel);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Low stock alert job completed at {Time}. Found {Count} low stock products", 
                DateTime.UtcNow, lowStockProducts.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in low stock alert job");
            throw;
        }
    }
}

