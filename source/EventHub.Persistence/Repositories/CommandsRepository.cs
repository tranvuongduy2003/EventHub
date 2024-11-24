using System.Linq.Expressions;
using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.Repositories;

public class CommandsRepository : RepositoryBase<Command>, ICommandsRepository
{
    private readonly ApplicationDbContext _context;

    public CommandsRepository(ApplicationDbContext context) : base(
        context)
    {
        _context = context;
    }

    public async Task<bool> ExistAsync(string id)
    {
        return await _context.Set<Command>().AnyAsync(x => x.GetType().GetProperty("Id")!.ToString() == id);
    }

    public async Task<Command> GetByIdAsync(string id)
    {
        return await _context.Set<Command>().FindAsync(id);
    }

    public async Task<Command> GetByIdAsync(string id, params Expression<Func<Command, object>>[] includeProperties)
    {
        return await _context.Set<Command>().FindAsync(id, includeProperties);
    }
}
