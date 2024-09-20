using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Event;

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
    /// <param name="eventId">
    /// A <see cref="Guid"/> representing the unique identifier of the event to be updated.
    /// </param>
    /// <param name="request">
    /// An <see cref="UpdateEventDto"/> object containing the updated event details.
    /// </param>
    /// <param name="authorId">
    /// A <see cref="Guid"/> representing the unique identifier of the user updating the event.
    /// </param>
    public UpdateEventCommand(Guid eventId, UpdateEventDto request, Guid authorId)
        => (EventId, Event, AuthorId) = (eventId, request, authorId);

    /// <summary>
    /// Gets or sets the unique identifier of the event to be updated.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier of the event.
    /// </value>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the updated event details.
    /// </summary>
    /// <value>
    /// An <see cref="UpdateEventDto"/> object representing the updated event data.
    /// </value>
    public UpdateEventDto Event { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who is updating the event.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the author's ID.
    /// </value>
    public Guid AuthorId { get; set; }
}