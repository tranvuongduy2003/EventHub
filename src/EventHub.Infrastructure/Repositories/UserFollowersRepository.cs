using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class UserFollowersRepository : RepositoryBase<UserFollower>, IUserFollowersRepository
{
    public UserFollowersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}