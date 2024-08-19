using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IPaymentsRepository : IRepositoryBase<Payment>
{
}