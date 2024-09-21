using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class ReviewsRepository : RepositoryBase<Review>, IReviewsRepository
{
    public ReviewsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}