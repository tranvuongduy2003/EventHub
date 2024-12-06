using EventHub.Domain.Aggregates.ReviewAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedReviewsRepository : ICachedRepositoryBase<Review>
{
}