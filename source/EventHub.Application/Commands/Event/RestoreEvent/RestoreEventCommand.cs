using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.RestoreEvent;

/// <summary>
/// Represents a command to restore a list of events for a user.
/// </summary>
/// <remarks>
/// This command is used to restore previously deleted or hidden events for a specific user.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user requesting the event restoration.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
/// <summary>
/// Gets the list of unique identifiers for the events that are being restored.
/// </summary>
/// <param name="Events">
/// A list of <see cref="Guid"/> representing the unique identifiers of the events.
/// </param>
public record RestoreEventCommand(Guid UserId, List<Guid> Events) : ICommand;