using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ICommandInFunctionsRepository : IRepositoryBase<CommandInFunction>
{
}
