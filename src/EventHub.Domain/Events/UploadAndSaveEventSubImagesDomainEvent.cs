using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that is triggered when sub-images for an event are uploaded and saved.
/// </summary>
/// <remarks>
/// This event captures the details of the event and the uploaded sub-images.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the domain event.
/// </param>
/// <summary>
/// Gets the unique identifier of the event for which sub-images are uploaded.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
/// <summary>
/// Gets the collection of sub-images that have been uploaded for the event.
/// </summary>
/// <param name="SubImages">
/// An <see cref="IFormFileCollection"/> representing the collection of uploaded sub-images.
/// </param>
public record UploadAndSaveEventSubImagesDomainEvent
    (Guid Id, Guid EventId, IFormFileCollection SubImages) : DomainEvent(Id);