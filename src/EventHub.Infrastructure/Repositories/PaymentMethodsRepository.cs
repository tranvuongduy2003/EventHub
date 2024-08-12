using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class PaymentMethodsRepository : RepositoryBase<PaymentMethod>, IPaymentMethodsRepository
{
    public PaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}