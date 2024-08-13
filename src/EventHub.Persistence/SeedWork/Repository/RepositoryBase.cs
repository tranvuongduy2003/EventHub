using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.SeedWork.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
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

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
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