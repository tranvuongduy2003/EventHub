using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class CommandsRepository : RepositoryBase<Command>, ICommandsRepository
{
    public CommandsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}