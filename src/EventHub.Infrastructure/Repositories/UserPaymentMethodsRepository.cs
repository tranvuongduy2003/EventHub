using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class UserPaymentMethodsRepository : RepositoryBase<UserPaymentMethod>, IUserPaymentMethodsRepository
{
    public UserPaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}