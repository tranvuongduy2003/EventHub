using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when a user unfollows another user.
/// </summary>
/// <remarks>
/// This event captures the details of the follower and the user being unfollowed.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the follower.
/// </summary>
/// <param name="FollowerId">
/// A <see cref="Guid"/> representing the unique identifier of the user who is unfollowing.
/// </param>
/// <summary>
/// Gets the unique identifier of the followed user.
/// </summary>
/// <param name="FollowedUserId">
/// A <see cref="Guid"/> representing the unique identifier of the user being unfollowed.
/// </param>
public record UnfollowUserDomainEvent(Guid Id, Guid FollowerId, Guid FollowedUserId) : DomainEvent(Id);