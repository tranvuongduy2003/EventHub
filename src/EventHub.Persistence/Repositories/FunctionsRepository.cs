using System.Linq.Expressions;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Persistence.Repositories;

public class FunctionsRepository : RepositoryBase<Function>, IFunctionsRepository
{
    private readonly ApplicationDbContext _context;

    public FunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
        _context = context;
    }

    public async Task<bool> ExistAsync(string id)
    {
        return await _context.Set<Function>().AnyAsync(x => x.GetType().GetProperty("Id").ToString() == id);
    }

    public async Task<Function> GetByIdAsync(string id)
    {
        return await _context.Set<Function>().FindAsync(id);
    }

    public async Task<Function> GetByIdAsync(string id, params Expression<Func<Function, object>>[] includeProperties)
    {
        return await _context.Set<Function>().FindAsync(id, includeProperties);
    }
}