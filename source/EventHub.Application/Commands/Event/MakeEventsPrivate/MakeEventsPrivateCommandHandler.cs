using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.MakeEventsPrivate;

public class MakeEventsPrivateCommandHandler : ICommandHandler<MakeEventsPrivateCommand>
{
    public MakeEventsPrivateCommandHandler()
    {
    }

    public async Task Handle(MakeEventsPrivateCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            Domain.AggregateModels.EventAggregate.Event.MakeEventsPrivate(request.UserId, request.Events);
        }, cancellationToken);
    }
}
