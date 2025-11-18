using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IRepository<Product>? _products;
    private IRepository<Category>? _categories;
    private IRepository<Supplier>? _suppliers;
    private IRepository<Warehouse>? _warehouses;
    private IRepository<InventoryTransaction>? _inventoryTransactions;
    private IRepository<User>? _users;
    private IRepository<ActivityLog>? _activityLogs;
    private IRepository<LowStockAlert>? _lowStockAlerts;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
    public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
    public IRepository<Supplier> Suppliers => _suppliers ??= new Repository<Supplier>(_context);
    public IRepository<Warehouse> Warehouses => _warehouses ??= new Repository<Warehouse>(_context);
    public IRepository<InventoryTransaction> InventoryTransactions => _inventoryTransactions ??= new Repository<InventoryTransaction>(_context);
    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<ActivityLog> ActivityLogs => _activityLogs ??= new Repository<ActivityLog>(_context);
    public IRepository<LowStockAlert> LowStockAlerts => _lowStockAlerts ??= new Repository<LowStockAlert>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

