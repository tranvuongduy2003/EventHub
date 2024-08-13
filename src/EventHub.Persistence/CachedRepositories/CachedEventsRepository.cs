using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedEventsRepository : CachedRepositoryBase<Event>
{
    public CachedEventsRepository(ApplicationDbContext context, RepositoryBase<Event> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}