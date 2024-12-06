using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class UserPaymentMethodsRepository : RepositoryBase<UserPaymentMethod>, IUserPaymentMethodsRepository
{
    public UserPaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}