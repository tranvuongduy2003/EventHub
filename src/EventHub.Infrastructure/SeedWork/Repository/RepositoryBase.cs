using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Interfaces;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Infrastructor.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructor.SeedWork.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return !trackChanges
            ? _context.Set<T>().AsNoTracking().Where(e => e.DeletedAt != null)
            : _context.Set<T>().Where(e => e.DeletedAt != null);
    }

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges
            ? _context.Set<T>().Where(e => e.DeletedAt != null).Where(expression).AsNoTracking()
            : _context.Set<T>().Where(e => e.DeletedAt != null).Where(expression);
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
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