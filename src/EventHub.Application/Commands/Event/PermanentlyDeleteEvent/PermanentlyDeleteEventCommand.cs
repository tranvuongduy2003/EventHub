using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.PermanentlyDeleteEvent;

/// <summary>
/// Represents a command to permanently delete an event.
/// </summary>
/// <remarks>
/// This command is used to remove an event permanently from the system.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the event to be permanently deleted.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record PermanentlyDeleteEventCommand(Guid EventId) : ICommand;