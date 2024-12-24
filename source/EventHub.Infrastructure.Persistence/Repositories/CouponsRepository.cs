using EventHub.Domain.Aggregates.CouponAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class CouponsRepository : RepositoryBase<Coupon>, ICouponsRepository
{
    public CouponsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}
