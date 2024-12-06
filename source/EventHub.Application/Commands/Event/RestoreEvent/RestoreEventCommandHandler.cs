using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.RestoreEvent;

public class RestoreEventCommandHandler : ICommandHandler<RestoreEventCommand>
{
    public RestoreEventCommandHandler()
    {
    }

    public async Task Handle(RestoreEventCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            Domain.Aggregates.EventAggregate.Event.RestoreEvent(request.UserId, request.Events);
        }, cancellationToken);
    }
}
