using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class CommandsRepository : RepositoryBase<Command>, ICommandsRepository
{
    public CommandsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}