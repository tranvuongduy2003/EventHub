using System.Linq.Expressions;
using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.PermissionAggregate;

namespace EventHub.Abstractions.Repositories;

public interface ICommandsRepository : IRepositoryBase<Command>
{
    Task<bool> ExistAsync(string id);

    Task<Command> GetByIdAsync(string id);

    Task<Command> GetByIdAsync(string id, params Expression<Func<Command, object>>[] includeProperties);
}