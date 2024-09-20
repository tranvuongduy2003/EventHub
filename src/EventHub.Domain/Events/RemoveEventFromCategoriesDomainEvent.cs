using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when an event is removed from its associated categories.
/// </summary>
/// <remarks>
/// This event captures the details of the event being removed from the categories.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event being removed from the categories.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record RemoveEventFromCategoriesDomainEvent(Guid Id, Guid EventId) : DomainEvent(Id);