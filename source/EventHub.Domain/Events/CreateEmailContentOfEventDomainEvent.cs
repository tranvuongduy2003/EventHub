using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when the email content for an event is created.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the associated email content.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which the email content is being created.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the email content that has been created for the event.
/// </summary>
/// <param name="EmailContent">
/// A <see cref="CreateEmailContentDto"/> object containing the details of the email content for the event.
/// </param>
public record CreateEmailContentOfEventDomainEvent
    (Guid Id, Guid EventId, CreateEmailContentDto EmailContent) : DomainEvent(Id);