using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.UserAggregate;

public interface IUserFollowersRepository : IRepositoryBase<UserFollower>
{
}