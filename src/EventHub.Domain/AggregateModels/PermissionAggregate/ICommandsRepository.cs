using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.PermissionAggregate;

public interface ICommandsRepository : IRepositoryBase<Command>
{
}