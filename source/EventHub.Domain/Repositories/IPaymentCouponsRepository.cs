using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IPaymentCouponsRepository : IRepositoryBase<PaymentCoupon>
{
}
