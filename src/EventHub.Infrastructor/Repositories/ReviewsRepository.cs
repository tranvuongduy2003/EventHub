using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class ReviewsRepository : RepositoryBase<Review>, IReviewsRepository
{
    public ReviewsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}