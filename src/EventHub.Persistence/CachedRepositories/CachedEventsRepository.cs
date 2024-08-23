using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedEventsRepository : CachedRepositoryBase<Event>, ICachedEventsRepository
{
    public CachedEventsRepository(ApplicationDbContext context, RepositoryBase<Event> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}