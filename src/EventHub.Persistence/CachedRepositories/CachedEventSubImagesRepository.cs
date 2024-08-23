using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedEventSubImagesRepository : CachedRepositoryBase<EventSubImage>, ICachedEventSubImagesRepository
{
    public CachedEventSubImagesRepository(ApplicationDbContext context, IRepositoryBase<EventSubImage> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}