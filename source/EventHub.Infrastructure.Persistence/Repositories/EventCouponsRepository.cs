using EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class EventCouponsRepository : RepositoryBase<EventCoupon>, IEventCouponsRepository
{
    public EventCouponsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}
