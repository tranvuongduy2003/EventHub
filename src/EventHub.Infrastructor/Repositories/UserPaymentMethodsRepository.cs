using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class UserPaymentMethodsRepository : RepositoryBase<UserPaymentMethod>, IUserPaymentMethodsRepository
{
    public UserPaymentMethodsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}