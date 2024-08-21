using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.SeedWork.Repository;

/// <summary>
/// Provides a base implementation for a repository that includes caching capabilities.
/// </summary>
/// <typeparam name="T">The type of the entity managed by this repository. It must derive from <see cref="EntityBase"/>.</typeparam>
/// <remarks>
/// This class extends the functionality of the base repository by integrating caching operations.
/// It wraps an existing repository (decorated) and uses a caching service (cacheService) to manage cached data.
/// </remarks>
public class CachedRepositoryBase<T> : ICachedRepositoryBase<T> where T : EntityBase
{
    private readonly RepositoryBase<T> _decorated;
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedRepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="context">The database context used by the repository.</param>
    /// <param name="decorated">An instance of the base repository that is being decorated with caching capabilities.</param>
    /// <param name="cacheService">The caching service used to manage cached data.</param>
    /// <remarks>
    /// The constructor initializes the repository with a database context, a base repository instance, and a caching service.
    /// This setup allows the repository to use caching in addition to the standard repository operations.
    /// </remarks>
    public CachedRepositoryBase(ApplicationDbContext context, RepositoryBase<T> decorated, ICacheService cacheService)
    {
        _context = context;
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public async Task<IQueryable<T>> FindAll(bool trackChanges = false)
    {
        var key = $"{nameof(T)}";

        var items = await _cacheService.GetData<IQueryable<T>>(key);

        if (items == null || !items.Any())
        {
            items = !trackChanges
                ? Queryable.Where<T>(_context.Set<T>().AsNoTracking(), e => e.DeletedAt != null)
                : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt != null);

            await _cacheService.SetData<IQueryable<T>>(key, items, TimeSpan.FromMinutes(2));
        }

        return items;
    }

    public async Task<IQueryable<T>> FindAll(bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
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
    
    public Task<bool> ExistAsync(string id) 
        => _decorated.ExistAsync(id);

    public Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
        => _decorated.ExistAsync(expression);

    public async Task<T> GetByIdAsync(string id)
    {
        string key = $"{nameof(T)}-{id}";

        var entity = await _cacheService.GetData<T>(key);
        if (entity != null)
            return entity;

        entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
            await _cacheService.SetData<T>(key, entity, TimeSpan.FromMinutes(2));

        return entity;
    }

    public Task<T> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties)
        => _decorated.GetByIdAsync(id, includeProperties);

    public Task CreateAsync(T entity) => _decorated.CreateAsync(entity);

    public Task CreateListAsync(IEnumerable<T> entities) => _decorated.CreateListAsync(entities);

    public Task UpdateAsync(T entity) => _decorated.UpdateAsync(entity);

    public Task DeleteAsync(T entity) => _decorated.DeleteAsync(entity);

    public Task DeleteListAsync(IEnumerable<T> entities) => _decorated.DeleteListAsync(entities);

    public Task SoftDeleteAsync(T entity) => _decorated.SoftDeleteAsync(entity);

    public Task RestoreAsync(T entity) => _decorated.RestoreAsync(entity);
}