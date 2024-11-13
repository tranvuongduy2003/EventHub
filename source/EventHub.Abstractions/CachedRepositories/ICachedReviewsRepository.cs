using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.ReviewAggregate;

namespace EventHub.Abstractions.CachedRepositories;

public interface ICachedReviewsRepository : ICachedRepositoryBase<Review>
{
}