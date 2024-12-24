using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.CachedRepositories;

public interface ICachedReviewsRepository : ICachedRepositoryBase<Review>
{
}