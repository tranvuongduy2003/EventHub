using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class PaymentCouponsRepository : RepositoryBase<PaymentCoupon>, IPaymentCouponsRepository
{
    public PaymentCouponsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}
