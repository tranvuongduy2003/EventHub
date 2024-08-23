using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedReviewsRepository : CachedRepositoryBase<Review>, ICachedReviewsRepository
{
    public CachedReviewsRepository(ApplicationDbContext context, IRepositoryBase<Review> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}