using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedReviewsRepository : ICachedRepositoryBase<Review>
{
}