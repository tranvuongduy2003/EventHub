using System.Linq.Expressions;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface ICommandsRepository : IRepositoryBase<Command>
{
    Task<bool> ExistAsync(string id);
    
    Task<Command> GetByIdAsync(string id);
    
    Task<Command> GetByIdAsync(string id, params Expression<Func<Command, object>>[] includeProperties);
}