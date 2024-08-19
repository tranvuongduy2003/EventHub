using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class UserFollowersRepository : RepositoryBase<UserFollower>, IUserFollowersRepository
{
    public UserFollowersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}