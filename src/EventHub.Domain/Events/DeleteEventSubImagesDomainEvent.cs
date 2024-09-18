using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when sub-images of an event are deleted.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the deletion of its associated sub-images.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which the sub-images are deleted.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record DeleteEventSubImagesDomainEvent(Guid Id, Guid EventId) : DomainEvent(Id);