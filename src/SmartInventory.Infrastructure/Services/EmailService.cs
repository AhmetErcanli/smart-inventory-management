using Microsoft.Extensions.Logging;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Infrastructure.Services;

public class DummyEmailService : IEmailService
{
    private readonly ILogger<DummyEmailService> _logger;

    public DummyEmailService(ILogger<DummyEmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendLowStockAlertAsync(string toEmail, string productName, int currentQuantity, int minStockLevel, CancellationToken cancellationToken = default)
    {
        // Dummy implementation - just log the email
        _logger.LogWarning(
            "LOW STOCK ALERT - Email would be sent to: {Email}, Product: {ProductName}, Current Quantity: {CurrentQuantity}, Min Stock Level: {MinStockLevel}",
            toEmail, productName, currentQuantity, minStockLevel);

        // Simulate async email sending
        await Task.Delay(100, cancellationToken);
    }
}

