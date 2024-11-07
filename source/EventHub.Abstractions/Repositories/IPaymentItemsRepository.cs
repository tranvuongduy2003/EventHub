using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.PaymentAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IPaymentItemsRepository : IRepositoryBase<PaymentItem>
{
}