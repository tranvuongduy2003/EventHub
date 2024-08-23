using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.SeedWork.Repository;

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

    public async Task<IQueryable<T>> FindAll(bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>().AsNoTracking(), e => e.DeletedAt != null)
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt != null);
    }

    public async Task<IQueryable<T>> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = await FindAll(trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public async Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt != null).Where(expression).AsNoTracking()
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt != null).Where(expression);
    }

    public async Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = await FindByCondition(expression, trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        return await _context.Set<T>().AnyAsync(x => new Guid(x.GetType().GetProperty("Id").ToString()) == id);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().AnyAsync(expression);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        return await _context.Set<T>().FindAsync(id, includeProperties);
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task CreateListAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public async Task UpdateAsync(T entity)
    {
        if (_context.Entry(entity).State != EntityState.Unchanged)
            _context.Entry(entity).CurrentValues.SetValues(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task DeleteListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public async Task SoftDeleteAsync(T entity)
    {
        _context.Entry(entity).Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
    }

    public async Task RestoreAsync(T entity)
    {
        _context.Entry(entity).Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = null;
    }
}