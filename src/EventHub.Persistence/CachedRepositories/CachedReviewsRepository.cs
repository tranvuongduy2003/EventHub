using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedReviewsRepository : CachedRepositoryBase<Review>, ICachedReviewsRepository
{
    public CachedReviewsRepository(ApplicationDbContext context, RepositoryBase<Review> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}