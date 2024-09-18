using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when an event is added to multiple categories.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the associated categories.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event being added to categories.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the list of category identifiers to which the event is being added.
/// </summary>
/// <param name="Categories">
/// A list of <see cref="Guid"/> representing the unique identifiers of the categories.
/// </param>
public record AddEventToCategoriesDomainEvent(Guid Id, Guid EventId, List<Guid> Categories) : DomainEvent(Id);