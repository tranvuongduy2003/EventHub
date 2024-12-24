using EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IEventCouponsRepository : IRepositoryBase<EventCoupon>
{
}
