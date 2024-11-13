using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.MakeEventsPublic;

public class MakeEventsPublicCommandHandler : ICommandHandler<MakeEventsPublicCommand>
{
    public MakeEventsPublicCommandHandler()
    {
    }

    public async Task Handle(MakeEventsPublicCommand request, CancellationToken cancellationToken)
    {
        await Domain.AggregateModels.EventAggregate.Event.MakeEventsPublic(request.UserId, request.Events);
    }
}