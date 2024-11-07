using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.PaymentAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IPaymentsRepository : IRepositoryBase<Payment>
{
}