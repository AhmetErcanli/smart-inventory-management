namespace SmartInventory.Domain.Interfaces;

public interface IEmailService
{
    Task SendLowStockAlertAsync(string toEmail, string productName, int currentQuantity, int minStockLevel, CancellationToken cancellationToken = default);
}

