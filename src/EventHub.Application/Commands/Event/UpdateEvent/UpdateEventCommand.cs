using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Application.Commands.Event.UpdateEvent;

public class UpdateEventCommand : ICommand
{
    public UpdateEventCommand(Guid eventId, UpdateEventDto request, Guid authorId)
        => (EventId, Event, AuthorId) = (eventId, request, authorId);

    public Guid EventId { get; set; }

    public UpdateEventDto Event { get; set; }

    public Guid AuthorId { get; set; }
}