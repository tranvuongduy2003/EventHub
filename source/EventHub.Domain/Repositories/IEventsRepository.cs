using System.Linq.Expressions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IEventsRepository : IRepositoryBase<Event>
{
    Task UpdateAccessStatusAsync(IQueryable<Event> events, bool isPrivate, CancellationToken cancellationToken);
    
    Task RestoreAsync(IQueryable<Event> events, CancellationToken cancellationToken);
    
    Task<Event> GetDeletedEventByIdAsync(Guid eventId, CancellationToken cancellationToken);
    
    IQueryable<Event> GetDeletedEvents(bool trackChanges = false);
    
    IQueryable<Event> GetDeletedEvents(Expression<Func<Event, bool>> expression, bool trackChanges = false);
}
