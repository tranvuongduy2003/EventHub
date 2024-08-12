using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class PaymentItemsRepository : RepositoryBase<PaymentItem>, IPaymentItemsRepository
{
    public PaymentItemsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}