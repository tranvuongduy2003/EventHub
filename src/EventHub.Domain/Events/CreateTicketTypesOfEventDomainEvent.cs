using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when ticket types are created for an event.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the associated ticket types.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which ticket types are being created.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the list of ticket types that have been created for the event.
/// </summary>
/// <param name="TicketTypes">
/// A list of <see cref="CreateTicketTypeDto"/> objects representing the ticket types created for the event.
/// </param>
public record CreateTicketTypesOfEventDomainEvent
    (Guid Id, Guid EventId, List<CreateTicketTypeDto> TicketTypes) : DomainEvent(Id);