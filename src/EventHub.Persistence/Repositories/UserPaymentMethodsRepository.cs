using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class UserPaymentMethodsRepository : RepositoryBase<UserPaymentMethod>, IUserPaymentMethodsRepository
{
    public UserPaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}