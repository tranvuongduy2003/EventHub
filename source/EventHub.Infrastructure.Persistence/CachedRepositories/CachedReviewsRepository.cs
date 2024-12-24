using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.CachedRepositories;

public class CachedReviewsRepository : CachedRepositoryBase<Review>, ICachedReviewsRepository
{
    public CachedReviewsRepository(ApplicationDbContext context, IRepositoryBase<Review> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}
