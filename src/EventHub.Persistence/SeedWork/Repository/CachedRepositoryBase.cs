using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.SeedWork.Repository;

public class CachedRepositoryBase<T> : ICachedRepositoryBase<T> where T : EntityBase
{
    private readonly RepositoryBase<T> _decorated;
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _context;

    public CachedRepositoryBase(ApplicationDbContext context, RepositoryBase<T> decorated, ICacheService cacheService)
    {
        _context = context;
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public async Task<IQueryable<T>> FindAllCached(bool trackChanges = false)
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

    public async Task<IQueryable<T>> FindAllCached(bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = await FindAllCached(trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public async Task<IQueryable<T>> FindCachedByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt != null).Where(expression).AsNoTracking()
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt != null).Where(expression);
    }

    public async Task<IQueryable<T>> FindCachedByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = await FindCachedByCondition(expression, trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
        => _decorated.FindAll(trackChanges);

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        => _decorated.FindAll(trackChanges, includeProperties);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        => _decorated.FindByCondition(expression, trackChanges);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        => _decorated.FindByCondition(expression, trackChanges, includeProperties);

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