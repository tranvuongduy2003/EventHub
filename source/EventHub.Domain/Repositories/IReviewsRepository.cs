using EventHub.Domain.Aggregates.ReviewAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IReviewsRepository : IRepositoryBase<Review>
{
}