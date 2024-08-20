using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.SeedWork.Repository;

public interface IRepositoryBase<T> where T : EntityBase
{
    IQueryable<T> FindAll(bool trackChanges = false);

    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    Task<bool> ExistAsync(string id);
    
    Task<bool> ExistAsync(Expression<Func<T, bool>> expression);
    
    Task<T> GetByIdAsync(string id);

    Task<T> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties);

    Task CreateAsync(T entity);

    Task CreateListAsync(IEnumerable<T> entities);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task DeleteListAsync(IEnumerable<T> entities);

    Task SoftDeleteAsync(T entity);

    Task RestoreAsync(T entity);
}