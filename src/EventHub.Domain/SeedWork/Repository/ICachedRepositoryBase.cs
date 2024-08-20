using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;

namespace EventHub.Domain.SeedWork.Repository;

public interface ICachedRepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    Task<IQueryable<T>> FindAllCached(bool trackChanges = false);

    Task<IQueryable<T>> FindAllCached(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

    Task<IQueryable<T>> FindCachedByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    Task<IQueryable<T>> FindCachedByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);
}