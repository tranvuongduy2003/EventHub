using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.FavouriteEvent;

/// <summary>
/// Represents a command to mark an event as a favorite for a user.
/// </summary>
/// <remarks>
/// This command is used to add an event to the user's list of favorite events.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user who is marking the event as a favorite.
/// </summary>
/// <summary>
/// Gets the unique identifier of the event being marked as a favorite.
/// </summary>
/// <param name="EventId">
/// A <see cref="Guid"/> representing the unique identifier of the event.
/// </param>
public record FavouriteEventCommand(Guid EventId) : ICommand;
