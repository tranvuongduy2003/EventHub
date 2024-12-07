using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.UnfavouriteEvent;

/// <summary>
/// Represents a command to remove an event from a user's favorites.
/// </summary>
/// <remarks>
/// This command is used to unfavorite an event for a specific user.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who is unfavoriting the event.
/// </summary>
/// <summary>
/// Gets the unique identifier of the event being unfavorited.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record UnfavouriteEventCommand(Guid EventId) : ICommand;
