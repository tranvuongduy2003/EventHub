using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.PaymentAggregate;

public interface IPaymentsRepository : IRepositoryBase<Payment>
{
}