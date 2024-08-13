using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class PaymentMethodsRepository : RepositoryBase<PaymentMethod>, IPaymentMethodsRepository
{
    public PaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}