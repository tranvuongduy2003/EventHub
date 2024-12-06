using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IPaymentItemsRepository : IRepositoryBase<PaymentItem>
{
}