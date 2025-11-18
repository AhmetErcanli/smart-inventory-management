using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;
using System.Linq.Expressions;

namespace SmartInventory.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            // Load related entities if they exist
            var entry = _context.Entry(entity);
            foreach (var navigation in entry.Navigations)
            {
                if (!navigation.IsLoaded)
                {
                    await navigation.LoadAsync(cancellationToken);
                }
            }
        }
        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.ToListAsync(cancellationToken);
        // Load navigation properties
        foreach (var entity in entities)
        {
            var entry = _context.Entry(entity);
            foreach (var navigation in entry.Navigations)
            {
                if (!navigation.IsLoaded)
                {
                    await navigation.LoadAsync(cancellationToken);
                }
            }
        }
        return entities;
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entities = await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        // Load navigation properties
        foreach (var entity in entities)
        {
            var entry = _context.Entry(entity);
            foreach (var navigation in entry.Navigations)
            {
                if (!navigation.IsLoaded)
                {
                    await navigation.LoadAsync(cancellationToken);
                }
            }
        }
        return entities;
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        if (entity != null)
        {
            // Load related entities if they exist
            var entry = _context.Entry(entity);
            foreach (var navigation in entry.Navigations)
            {
                if (!navigation.IsLoaded)
                {
                    await navigation.LoadAsync(cancellationToken);
                }
            }
        }
        return entity;
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
            return await _dbSet.CountAsync(cancellationToken);
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }
}

