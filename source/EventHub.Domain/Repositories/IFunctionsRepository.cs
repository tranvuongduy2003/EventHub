using System.Linq.Expressions;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IFunctionsRepository : IRepositoryBase<Function>
{
    Task<bool> ExistAsync(string id);

    Task<Function> GetByIdAsync(string id);

    Task<Function> GetByIdAsync(string id, params Expression<Func<Function, object>>[] includeProperties);
}
