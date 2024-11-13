using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when the reasons associated with an event are deleted.
/// </summary>
/// <remarks>
/// This event captures the details of the event whose reasons for registration are being deleted.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event whose reasons for registration are being deleted.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record DeleteEventReasonsDomainEvent(Guid Id, Guid EventId) : DomainEvent(Id);