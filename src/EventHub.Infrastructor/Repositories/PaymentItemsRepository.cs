using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class PaymentItemsRepository : RepositoryBase<PaymentItem>, IPaymentItemsRepository
{
    public PaymentItemsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}