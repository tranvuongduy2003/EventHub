using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateEventCommand : ICommand<EventDto>
{
    public CreateEventCommand(CreateEventDto request, Guid authorId)
        => (Event, AuthorId) = (request, authorId);

    public CreateEventDto Event { get; set; }

    public Guid AuthorId { get; set; }
}