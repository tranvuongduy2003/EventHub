using System.Linq.Expressions;
using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.PermissionAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IFunctionsRepository : IRepositoryBase<Function>
{
    Task<bool> ExistAsync(string id);

    Task<Function> GetByIdAsync(string id);

    Task<Function> GetByIdAsync(string id, params Expression<Func<Function, object>>[] includeProperties);
}