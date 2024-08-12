using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

public interface IPaymentMethodsRepository : IRepositoryBase<PaymentMethod>
{
}