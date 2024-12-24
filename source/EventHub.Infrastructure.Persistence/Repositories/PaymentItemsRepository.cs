using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate.Entities;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class PaymentItemsRepository : RepositoryBase<PaymentItem>, IPaymentItemsRepository
{
    public PaymentItemsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}