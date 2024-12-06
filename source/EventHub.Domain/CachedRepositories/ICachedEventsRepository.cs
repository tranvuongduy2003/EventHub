using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedEventsRepository : ICachedRepositoryBase<Event>
{
}