using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class PaymentsRepository : RepositoryBase<Payment>, IPaymentsRepository
{
    public PaymentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}