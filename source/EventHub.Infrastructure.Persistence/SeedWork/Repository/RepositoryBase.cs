using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Persistence.SeedWork.Repository;

/// <summary>
/// Provides a base implementation for repository classes handling entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity managed by this repository. It must derive from <see cref="EntityBase"/>.</typeparam>
/// <remarks>
/// This base class implements common repository operations such as CRUD operations for entities of type <typeparamref name="T"/>.
/// It is designed to be extended by specific repository classes to provide entity-specific operations.
/// </remarks>
public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="context">The database context used for database operations.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is <c>null</c>.</exception>
    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>().AsNoTracking(), e => e.DeletedAt == null)
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null);
    }

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> items = FindAll(trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null).Where(expression).AsNoTracking()
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null).Where(expression);
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> items = FindByCondition(expression, trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        return await _context.Set<T>().AnyAsync(x => new Guid(x.GetType().GetProperty("Id")!.ToString() ?? "") == id && !x.IsDeleted);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().Where(x => !x.IsDeleted).AnyAsync(expression);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        T entity = await _context.Set<T>().FindAsync(id);
        if (entity is not null && !entity.IsDeleted)
        {
            return entity;
        }
        return null;
    }

    public async Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        T entity = await _context.Set<T>().FindAsync(id, includeProperties);
        if (entity is not null && !entity.IsDeleted)
        {
            return entity;
        }
        return null;
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task CreateListAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public async Task Update(T entity)
    {
        await Task.Run(() =>
        {
            if (_context.Entry(entity).State != EntityState.Unchanged)
            {
                _context.Entry(entity).CurrentValues.SetValues(entity);
            }
        });
    }

    public async Task Delete(T entity)
    {
        await Task.Run(() =>
        {
            _context.Set<T>().Remove(entity);
        });
    }

    public async Task DeleteList(IEnumerable<T> entities)
    {
        await Task.Run(() =>
        {
            _context.Set<T>().RemoveRange(entities);
        });
    }

    public async Task SoftDelete(T entity)
    {
        await Task.Run(() =>
        {
            _context.Entry(entity).Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            _context.Entry(entity).Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
        });
    }

    public async Task Restore(T entity)
    {
        await Task.Run(() =>
        {
            _context.Entry(entity).Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = false;
            _context.Entry(entity).Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = null;
        });
    }
}
