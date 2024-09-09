using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

public record UnfollowUserDomainEvent(Guid Id, Guid FollowerId, Guid FollowedUserId) : DomainEvent(Id);