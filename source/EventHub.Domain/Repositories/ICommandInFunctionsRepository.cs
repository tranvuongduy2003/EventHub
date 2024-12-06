using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ICommandInFunctionsRepository : IRepositoryBase<CommandInFunction>
{
}
