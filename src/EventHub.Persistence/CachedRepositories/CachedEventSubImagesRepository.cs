using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedEventSubImagesRepository : CachedRepositoryBase<EventSubImage>
{
    public CachedEventSubImagesRepository(ApplicationDbContext context, RepositoryBase<EventSubImage> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}