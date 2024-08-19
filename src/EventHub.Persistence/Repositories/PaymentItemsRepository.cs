using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class PaymentItemsRepository : RepositoryBase<PaymentItem>, IPaymentItemsRepository
{
    public PaymentItemsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}