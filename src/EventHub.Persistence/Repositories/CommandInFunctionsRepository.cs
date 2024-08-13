using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class CommandInFunctionsRepository : RepositoryBase<CommandInFunction>, ICommandInFunctionsRepository
{
    public CommandInFunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}