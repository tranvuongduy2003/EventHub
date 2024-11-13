using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when the email content of an event is updated.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the updated email content.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the domain event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which the email content is updated.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the updated email content for the event.
/// </summary>
/// <param name="EmailContent">
/// A <see cref="CreateEmailContentDto"/> object representing the updated email content for the event.
/// </param>
public record UpdateEmailContentOfEventDomainEvent
    (Guid Id, Guid EventId, UpdateEmailContentDto EmailContent) : DomainEvent(Id);