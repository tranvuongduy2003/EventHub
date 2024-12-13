using System.Linq.Expressions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedEventsRepository : ICachedRepositoryBase<Event>
{
    Task UpdateAccessStatusAsync(IQueryable<Event> events, bool isPrivate, CancellationToken cancellationToken);
    
    Task RestoreAsync(IQueryable<Event> events, CancellationToken cancellationToken);
}
