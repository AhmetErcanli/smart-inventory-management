namespace SmartInventory.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Entities.Product> Products { get; }
    IRepository<Entities.Category> Categories { get; }
    IRepository<Entities.Supplier> Suppliers { get; }
    IRepository<Entities.Warehouse> Warehouses { get; }
    IRepository<Entities.InventoryTransaction> InventoryTransactions { get; }
    IRepository<Entities.User> Users { get; }
    IRepository<Entities.ActivityLog> ActivityLogs { get; }
    IRepository<Entities.LowStockAlert> LowStockAlerts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

