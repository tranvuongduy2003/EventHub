using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

public interface IPaymentItemsRepository : IRepositoryBase<PaymentItem>
{
}