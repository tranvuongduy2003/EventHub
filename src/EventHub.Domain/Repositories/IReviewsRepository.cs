using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IReviewsRepository : IRepositoryBase<Review>
{
}