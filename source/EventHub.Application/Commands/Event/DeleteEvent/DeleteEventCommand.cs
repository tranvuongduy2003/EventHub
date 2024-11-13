using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.DeleteEvent;

/// <summary>
/// Represents a command to delete an event.
/// </summary>
/// <remarks>
/// This command is used to delete an event by its unique identifier.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who is deleting the event.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
/// <summary>
/// Gets the unique identifier of the event to be deleted.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event to be deleted.
/// </param>
public record DeleteEventCommand(Guid UserId, Guid EventId) : ICommand;