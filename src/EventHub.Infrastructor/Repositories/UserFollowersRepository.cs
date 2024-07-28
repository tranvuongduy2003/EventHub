using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class UserFollowersRepository : RepositoryBase<UserFollower>, IUserFollowersRepository
{
    public UserFollowersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}