using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when a user's password is changed.
/// </summary>
/// <remarks>
/// This event captures the details of the password change, including the old and new passwords.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the unique identifier of the user whose password has been changed.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
/// <summary>
/// Gets the old password of the user before the change.
/// </summary>
/// <param name="OldPassword">
/// A string representing the old password of the user.
/// </param>
/// <summary>
/// Gets the new password that the user has set.
/// </summary>
/// <param name="NewPassword">
/// A string representing the new password of the user.
/// </param>
public record ChangeUserPasswordDomainEvent(Guid Id, Guid UserId, string OldPassword, string NewPassword) : DomainEvent(Id);