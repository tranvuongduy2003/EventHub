using EventHub.Abstractions;
using EventHub.Abstractions.CachedRepositories;
using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedReviewsRepository : CachedRepositoryBase<Review>, ICachedReviewsRepository
{
    public CachedReviewsRepository(ApplicationDbContext context, IRepositoryBase<Review> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}