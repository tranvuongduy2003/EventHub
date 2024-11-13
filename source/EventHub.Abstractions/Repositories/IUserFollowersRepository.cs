using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.UserAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IUserFollowersRepository : IRepositoryBase<UserFollower>
{
}