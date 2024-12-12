using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.MakeEventsPublic;

/// <summary>
/// Represents a command to mark a list of events as private.
/// </summary>
/// <remarks>
/// This command is used to change the visibility of specified events to private.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who is marking the events as private.
/// </summary>
/// <summary>
/// Gets the list of unique identifiers for the events to be made private.
/// </summary>
/// <param name="Events">
/// A list of <see cref="Guid"/> representing the unique identifiers of the events.
/// </param>
public record MakeEventsPublicCommand(List<Guid> Events) : ICommand;
