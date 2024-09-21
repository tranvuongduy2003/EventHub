using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.ReviewAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IReviewsRepository : IRepositoryBase<Review>
{
}