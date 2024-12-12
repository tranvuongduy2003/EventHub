using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when a list of events is marked as public.
/// </summary>
/// <remarks>
/// This event captures the details of the events being made public.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the list of unique identifiers for the events that are being marked as public.
/// </summary>
/// <param name="Events">
/// A list of <see cref="Guid"/> representing the unique identifiers of the events.
/// </param>
public record MakeEventsPublicDomainEvent(Guid Id, List<Guid> Events) : DomainEvent(Id);
