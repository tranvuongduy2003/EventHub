using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.PermissionAggregate;

namespace EventHub.Abstractions.Repositories;

public interface ICommandInFunctionsRepository : IRepositoryBase<CommandInFunction>
{
}