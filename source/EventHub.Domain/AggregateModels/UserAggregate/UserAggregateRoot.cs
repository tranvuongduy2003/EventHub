using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;

namespace EventHub.Domain.AggregateModels.UserAggregate;

public class UserAggregateRoot : AggregateRoot
{
    public static void ChangeUserPassword(Guid userId, string oldPassword, string newPassword)
    {
        new UserAggregateRoot().RaiseDomainEvent(
            new ChangeUserPasswordDomainEvent(Guid.NewGuid(), userId, oldPassword, newPassword));
    }

    public static void FollowUser(Guid followerId, Guid followedUserId)
    {
        new UserAggregateRoot().RaiseDomainEvent(
            new FollowUserDomainEvent(Guid.NewGuid(), followerId, followedUserId));
    }

    public static void UnfollowUser(Guid followerId, Guid followedUserId)
    {
        new UserAggregateRoot().RaiseDomainEvent(
            new UnfollowUserDomainEvent(Guid.NewGuid(), followerId, followedUserId));
    }
}
