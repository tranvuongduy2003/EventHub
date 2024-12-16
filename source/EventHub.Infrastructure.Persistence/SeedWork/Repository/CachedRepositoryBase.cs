using System.Linq.Expressions;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using EventHub.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Persistence.SeedWork.Repository;

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
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<T> _decorated;

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
    public CachedRepositoryBase(ApplicationDbContext context, IRepositoryBase<T> decorated, ICacheService cacheService)
    {
        _context = context;
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        string key = $"{typeof(T).Name}";

        if (_cacheService.GetData<List<T>>(key).GetAwaiter().GetResult() is not IQueryable<T> items || !items.Any())
        {
            items = !trackChanges
                ? Queryable.Where<T>(_context.Set<T>().AsNoTracking(), e => e.DeletedAt == null)
                : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null);

            _cacheService.SetData<IQueryable<T>>(key, items, TimeSpan.FromMinutes(2)).GetAwaiter();
        }

        return items;
    }

    public IQueryable<T> FindAll(bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> items = FindAll(trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        string key = $"{typeof(T).Name}";

        if (_cacheService.GetData<List<T>>(key).GetAwaiter().GetResult() is not IQueryable<T> items || !items.Any())
        {
            items = !trackChanges
                ? Queryable.Where<T>(_context.Set<T>().AsNoTracking(), e => e.DeletedAt == null)
                : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null);

            _cacheService.SetData<IQueryable<T>>(key, items, TimeSpan.FromMinutes(2)).GetAwaiter();

            items = items.Where(expression);
        }

        return items;
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

    public Pagination<T> PaginatedFind(PaginationFilter filter, bool trackChanges = false)
    {
        IQueryable<T> query = FindAll(trackChanges);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Pagination<T> PaginatedFind(PaginationFilter filter, Func<IQueryable<T>, IQueryable<T>> includePaths,
        bool trackChanges = false)
    {
        IQueryable<T> query = FindAll(trackChanges);

        // Apply includes if specified
        query = includePaths(query);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Pagination<T> PaginatedFindByCondition(Expression<Func<T, bool>> expression, PaginationFilter filter,
        bool trackChanges = false)
    {
        IQueryable<T> query = FindByCondition(expression, trackChanges);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Pagination<T> PaginatedFindByCondition(Expression<Func<T, bool>> expression, PaginationFilter filter,
        Func<IQueryable<T>, IQueryable<T>> includePaths,
        bool trackChanges = false)
    {
        IQueryable<T> query = FindByCondition(expression, trackChanges);

        // Apply includes if specified
        query = includePaths(query);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Task<bool> ExistAsync(Guid id)
        => _decorated.ExistAsync(id);

    public Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
        => _decorated.ExistAsync(expression);

    public async Task<T> GetByIdAsync(Guid id)
    {
        string key = $"{typeof(T).Name}-{id}";

        T entity = await _cacheService.GetData<T>(key);
        if (entity != null)
        {
            return entity;
        }

        entity = await _decorated.GetByIdAsync(id);
        if (entity != null)
        {
            await _cacheService.SetData<T>(key, entity, TimeSpan.FromMinutes(2));
        }

        return entity;
    }

    public Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        => _decorated.GetByIdAsync(id, includeProperties);

    public async Task CreateAsync(T entity)
    {
        await _decorated.CreateAsync(entity);

        string key = $"{typeof(T).Name}";
        await _cacheService.RemoveData(key);
    }

    public async Task CreateListAsync(IEnumerable<T> entities)
    {
        await _decorated.CreateListAsync(entities);

        string key = $"{typeof(T).Name}";
        await _cacheService.RemoveData(key);
    }

    public async Task Update(T entity)
    {
        await _decorated.Update(entity);

        string listKey = $"{typeof(T).Name}";
        await _cacheService.RemoveData(listKey);

        string entityKey = $"{typeof(T).Name}-{((dynamic)entity).Id}";
        await _cacheService.RemoveData(entityKey);
    }

    public async Task Delete(T entity)
    {
        await _decorated.Delete(entity);

        string listKey = $"{typeof(T).Name}";
        await _cacheService.RemoveData(listKey);

        string entityKey = $"{typeof(T).Name}-{((dynamic)entity).Id}";
        await _cacheService.RemoveData(entityKey);
    }

    public async Task DeleteList(IEnumerable<T> entities)
    {
        string listKey = $"{typeof(T).Name}";
        await _decorated.DeleteList(entities);
        await _cacheService.RemoveData(listKey);

        foreach (T entity in entities)
        {
            string entityKey = $"{typeof(T).Name}-{((dynamic)entity).Id}";
            await _cacheService.RemoveData(entityKey);
        }
    }

    public async Task SoftDelete(T entity)
    {
        await _decorated.SoftDelete(entity);

        string listKey = $"{typeof(T).Name}";
        await _cacheService.RemoveData(listKey);

        string entityKey = $"{typeof(T).Name}-{((dynamic)entity).Id}";
        await _cacheService.RemoveData(entityKey);
    }

    public async Task Restore(T entity)
    {
        await _decorated.Restore(entity);

        string listKey = $"{typeof(T).Name}";
        await _cacheService.RemoveData(listKey);
    }
}
