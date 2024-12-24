using EventHub.Domain.Aggregates.CouponAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ICouponsRepository : IRepositoryBase<Coupon>
{
}
