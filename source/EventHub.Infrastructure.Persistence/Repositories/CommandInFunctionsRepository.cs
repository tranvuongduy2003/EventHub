using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class CommandInFunctionsRepository : RepositoryBase<CommandInFunction>, ICommandInFunctionsRepository
{
    public CommandInFunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}