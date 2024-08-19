using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IUserFollowersRepository : IRepositoryBase<UserFollower>
{
}