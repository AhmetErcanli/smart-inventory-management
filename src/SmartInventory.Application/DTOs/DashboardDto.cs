namespace SmartInventory.Application.DTOs;

public class DashboardDto
{
    public int TotalProducts { get; set; }
    public decimal TotalStockValue { get; set; }
    public List<LowStockProductDto> TopLowStockProducts { get; set; } = new();
    public List<MonthlyTransactionDto> MonthlyTransactions { get; set; } = new();
    public List<CategoryDistributionDto> CategoryDistribution { get; set; } = new();
}

public class LowStockProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int MinStockLevel { get; set; }
}

public class MonthlyTransactionDto
{
    public string Month { get; set; } = string.Empty;
    public int StockIn { get; set; }
    public int StockOut { get; set; }
}

public class CategoryDistributionDto
{
    public string CategoryName { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public int TotalQuantity { get; set; }
}

