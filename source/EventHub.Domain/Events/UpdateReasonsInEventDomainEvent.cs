using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when the reasons for an event are updated.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the updated reasons for registration.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which the reasons are updated.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the updated list of reasons for registration for the event.
/// </summary>
/// <param name="Reasons">
/// A list of <see cref="string"/> representing the updated reasons for registering for the event.
/// </param>
public record UpdateReasonsInEventDomainEvent(Guid Id, Guid EventId, List<string> Reasons) : DomainEvent(Id);