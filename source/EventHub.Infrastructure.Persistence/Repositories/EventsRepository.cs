using System.Linq.Expressions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Repositories;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class EventsRepository : RepositoryBase<Event>, IEventsRepository
{
    private readonly ApplicationDbContext _context;

    public EventsRepository(ApplicationDbContext context) : base(
        context)
    {
        _context = context;
    }

    public Task UpdateAccessStatusAsync(IQueryable<Event> events, bool isPrivate,
        CancellationToken cancellationToken) =>
        events.ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsPrivate, isPrivate), cancellationToken);

    public Task RestoreAsync(IQueryable<Event> events, CancellationToken cancellationToken) =>
        events.ExecuteUpdateAsync(setters => setters
            .SetProperty(e => e.IsDeleted, false)
            .SetProperty(e => e.DeletedAt, (DateTime?)null), cancellationToken);

    public async Task<Event> GetDeletedEventByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        Event @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken);
        return @event ?? null;
    }

    public IQueryable<Event> GetDeletedEvents(bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<Event>(_context.Set<Event>().AsNoTracking(), e => e.IsDeleted)
            : Queryable.Where<Event>(_context.Set<Event>(), e => e.IsDeleted);
    }

    public IQueryable<Event> GetDeletedEvents(Expression<Func<Event, bool>> expression,
        bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<Event>(_context.Set<Event>(), e => e.IsDeleted).Where(expression).AsNoTracking()
            : Queryable.Where<Event>(_context.Set<Event>(), e => e.IsDeleted).Where(expression);
    }

    public Pagination<Event> GetPaginatedDeletedEvents(PaginationFilter filter, bool trackChanges = false)
    {
        IQueryable<Event> query = GetDeletedEvents(trackChanges);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Pagination<Event> GetPaginatedDeletedEvents(PaginationFilter filter, Func<IQueryable<Event>, IQueryable<Event>> includePaths,
        bool trackChanges = false)
    {
        IQueryable<Event> query = FindAll(trackChanges);

        // Apply includes if specified
        query = includePaths(query);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Pagination<Event> GetPaginatedDeletedEvents(Expression<Func<Event, bool>> expression, PaginationFilter filter,
        bool trackChanges = false)
    {
        IQueryable<Event> query = FindByCondition(expression, trackChanges);

        return PagingHelper.QueryPaginate(filter, query);
    }

    public Pagination<Event> GetPaginatedDeletedEvents(Expression<Func<Event, bool>> expression, PaginationFilter filter,
        Func<IQueryable<Event>, IQueryable<Event>> includePaths,
        bool trackChanges = false)
    {
        IQueryable<Event> query = FindByCondition(expression, trackChanges);

        // Apply includes if specified
        query = includePaths(query);

        return PagingHelper.QueryPaginate(filter, query);
    }
}
