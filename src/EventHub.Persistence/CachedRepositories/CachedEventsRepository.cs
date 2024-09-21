using EventHub.Abstractions;
using EventHub.Abstractions.CachedRepositories;
using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedEventsRepository : CachedRepositoryBase<Event>, ICachedEventsRepository
{
    public CachedEventsRepository(ApplicationDbContext context, IRepositoryBase<Event> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}