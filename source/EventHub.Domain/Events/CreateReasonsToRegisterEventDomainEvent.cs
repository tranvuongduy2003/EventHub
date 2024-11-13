using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when reasons to register for an event are created.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the associated reasons for registration.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which reasons to register are created.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the list of reasons to register for the event.
/// </summary>
/// <param name="Reasons">
/// A list of <see cref="string"/> representing the reasons for registering for the event.
/// </param>
public record CreateReasonsToRegisterEventDomainEvent(Guid Id, Guid EventId, List<string> Reasons) : DomainEvent(Id);