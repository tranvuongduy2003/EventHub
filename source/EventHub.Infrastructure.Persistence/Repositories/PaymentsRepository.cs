using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class PaymentsRepository : RepositoryBase<Payment>, IPaymentsRepository
{
    public PaymentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}