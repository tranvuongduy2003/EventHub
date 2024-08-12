using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class PaymentsRepository : RepositoryBase<Payment>, IPaymentsRepository
{
    public PaymentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}