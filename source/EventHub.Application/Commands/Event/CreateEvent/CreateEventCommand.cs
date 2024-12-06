using EventHub.Application.DTOs.Event;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Enums.Event;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Event.CreateEvent;

/// <summary>
/// Represents a command to create a new event.
/// </summary>
/// <remarks>
/// This command is used to create a new event with the specified details and the author's information.
/// </remarks>
public class CreateEventCommand : ICommand<EventDto>
{
    public CreateEventCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEventCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// A <see cref="CreateEventDto"/> object containing the details of the event to be created.
    /// </param>
    /// <param name="authorId">
    /// A <see cref="Guid"/> representing the unique identifier of the user who is creating the event.
    /// </param>
    public CreateEventCommand(CreateEventDto request, Guid authorId)
    {
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
        EmailContent = request.EmailContent != null ? new CreateEmailContentCommand(request.EmailContent) : null;
        AuthorId = authorId;
    }

    public IFormFile CoverImage { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public double? Promotion { get; set; }

    public EEventCycleType EventCycleType { get; set; }

    public EEventPaymentType EventPaymentType { get; set; }

    public bool IsPrivate { get; set; }

    public List<Guid> Categories { get; set; }

    public IEnumerable<string>? TicketTypes { get; set; }

    public IEnumerable<string>? Reasons { get; set; }

    public IFormFileCollection? EventSubImages { get; set; }

    public CreateEmailContentCommand? EmailContent { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who is creating the event.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the author's ID.
    /// </value>
    public Guid AuthorId { get; set; }
}
