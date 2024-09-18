using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when the categories of an event are updated.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the updated categories.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which the categories are updated.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the list of updated categories for the event.
/// </summary>
/// <param name="Categories">
/// A list of <see cref="Guid"/> representing the updated categories of the event.
/// </param>
public record UpdateCategoriesInEventDomainEvent(Guid Id, Guid EventId, List<Guid> Categories) : DomainEvent(Id);