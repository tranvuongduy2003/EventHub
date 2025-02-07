using System.Linq.Expressions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Domain.Repositories;

public interface IEventsRepository : IRepositoryBase<Event>
{
    Task UpdateAccessStatusAsync(IQueryable<Event> events, bool isPrivate, CancellationToken cancellationToken);

    Task RestoreAsync(IQueryable<Event> events, CancellationToken cancellationToken);

    Task<Event> GetDeletedEventByIdAsync(Guid eventId, CancellationToken cancellationToken);

    IQueryable<Event> GetDeletedEvents(bool trackChanges = false);

    IQueryable<Event> GetDeletedEvents(Expression<Func<Event, bool>> expression, bool trackChanges = false);

    Pagination<Event> GetPaginatedDeletedEvents(PaginationFilter filter, bool trackChanges = false);

    Pagination<Event> GetPaginatedDeletedEvents(PaginationFilter filter, Func<IQueryable<Event>, IQueryable<Event>> includePaths,
        bool trackChanges = false);

    Pagination<Event> GetPaginatedDeletedEvents(Expression<Func<Event, bool>> expression, PaginationFilter filter,
        bool trackChanges = false);

    Pagination<Event> GetPaginatedDeletedEvents(Expression<Func<Event, bool>> expression, PaginationFilter filter,
        Func<IQueryable<Event>, IQueryable<Event>> includePaths,
        bool trackChanges = false);
}
