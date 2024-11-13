using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.UnfavouriteEvent;

public class UnfavouriteEventCommandHandler : ICommandHandler<UnfavouriteEventCommand>
{
    public UnfavouriteEventCommandHandler()
    {
    }

    public async Task Handle(UnfavouriteEventCommand request, CancellationToken cancellationToken)
    {
        await Domain.AggregateModels.EventAggregate.Event
            .UnfavouriteEvent(request.UserId, request.EventId);
    }
}