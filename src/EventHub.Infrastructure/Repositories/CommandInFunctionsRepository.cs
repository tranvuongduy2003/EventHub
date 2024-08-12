using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class CommandInFunctionsRepository : RepositoryBase<CommandInFunction>, ICommandInFunctionsRepository
{
    public CommandInFunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}