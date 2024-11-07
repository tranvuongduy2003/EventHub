using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when the ticket types in an event are updated.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the updated ticket types.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which the ticket types are updated.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the updated ticket types for the event.
/// </summary>
/// <param name="TicketTypes">
/// A list of <see cref="UpdateTicketTypeDto"/> objects representing the updated ticket types for the event.
/// </param>
public record UpdateTicketTypesInEventDomainEvent
    (Guid Id, Guid EventId, List<UpdateTicketTypeDto> TicketTypes) : DomainEvent(Id);