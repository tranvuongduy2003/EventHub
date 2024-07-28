using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class PaymentMethodsRepository : RepositoryBase<PaymentMethod>, IPaymentMethodsRepository
{
    public PaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}