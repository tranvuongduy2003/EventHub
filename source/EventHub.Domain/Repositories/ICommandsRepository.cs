using System.Linq.Expressions;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ICommandsRepository : IRepositoryBase<Command>
{
    Task<bool> ExistAsync(string id);

    Task<Command> GetByIdAsync(string id);

    Task<Command> GetByIdAsync(string id, params Expression<Func<Command, object>>[] includeProperties);
}
