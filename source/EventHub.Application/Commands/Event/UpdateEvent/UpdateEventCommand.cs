using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Event.UpdateEvent;

/// <summary>
/// Represents a command to update an existing event.
/// </summary>
/// <remarks>
/// This command is used to update an event with new details provided by the author.
/// </remarks>
public class UpdateEventCommand : ICommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEventCommand"/> class.
    /// </summary>
    public UpdateEventCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEventCommand"/> class.
    /// </summary>
    /// <param name="eventId">
    /// A <see cref="Guid"/> representing the unique identifier of the event to be updated.
    /// </param>
    /// <param name="request">
    /// An <see cref="UpdateEventDto"/> object containing the updated event details.
    /// </param>
    public UpdateEventCommand(Guid eventId, UpdateEventDto request)
    {
        EventId = eventId;
        // Copy properties from request
        Id = request.Id;
        CoverImage = request.CoverImage;
        Name = request.Name;
        Description = request.Description;
        Location = request.Location;
        StartTime = request.StartTime;
        EndTime = request.EndTime;
        Promotion = request.Promotion;
        EventCycleType = request.EventCycleType;
        EventPaymentType = request.EventPaymentType;
        IsPrivate = request.IsPrivate;
        Categories = request.Categories;
        TicketTypes = request.TicketTypes;
        Reasons = request.Reasons;
        EventSubImages = request.EventSubImages;
        EmailContent = request.EmailContent != null
            ? new UpdateEmailContentCommand(request.EmailContent)
            : null;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the event to be updated.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the event.
    /// </value>
    public Guid EventId { get; set; }

    public Guid Id { get; set; }

    public IFormFile CoverImage { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime EndTime { get; set; } = DateTime.UtcNow;

    public double? Promotion { get; set; } = 0;

    public EEventCycleType EventCycleType { get; set; } = EEventCycleType.ONETIME;

    public EEventPaymentType EventPaymentType { get; set; } = EEventPaymentType.FREE;

    public bool IsPrivate { get; set; }

    public List<Guid> Categories { get; set; } = new();

    public IEnumerable<string>? TicketTypes { get; set; } = new List<string>();

    public IEnumerable<string>? Reasons { get; set; } = new List<string>();

    public IFormFileCollection? EventSubImages { get; set; }

    public UpdateEmailContentCommand? EmailContent { get; set; }
}
