using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Application.Commands.Event.CreateEvent;

/// <summary>
/// Represents a command to create a new event.
/// </summary>
/// <remarks>
/// This command is used to create a new event with the specified details and the author's information.
/// </remarks>
public class CreateEventCommand : ICommand<EventDto>
{
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
        => (Event, AuthorId) = (request, authorId);

    /// <summary>
    /// Gets or sets the details of the event to be created.
    /// </summary>
    /// <value>
    /// A <see cref="CreateEventDto"/> object representing the event's data.
    /// </value>
    public CreateEventDto Event { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who is creating the event.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the author's ID.
    /// </value>
    public Guid AuthorId { get; set; }
}