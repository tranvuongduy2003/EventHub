using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class ReviewsRepository : RepositoryBase<Review>, IReviewsRepository
{
    public ReviewsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}