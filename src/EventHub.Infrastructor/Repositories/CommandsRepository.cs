using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class CommandsRepository : RepositoryBase<Command>, ICommandsRepository
{
    public CommandsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}