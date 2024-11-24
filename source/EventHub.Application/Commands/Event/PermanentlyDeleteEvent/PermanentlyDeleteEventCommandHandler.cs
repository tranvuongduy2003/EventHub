using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.PermanentlyDeleteEvent;

public class PermanentlyDeleteEventCommandHandler : ICommandHandler<PermanentlyDeleteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PermanentlyDeleteEventCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PermanentlyDeleteEventCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        _unitOfWork.Events.Delete(@event);

        Domain.AggregateModels.EventAggregate.Event
            .DeleteEventSubImages(@event.Id);

        Domain.AggregateModels.EventAggregate.Event
            .DeleteEmailContent(@event.Id);

        Domain.AggregateModels.EventAggregate.Event
            .DeleteEventTicketTypes(@event.Id);

        Domain.AggregateModels.EventAggregate.Event
            .RemoveEventFromCategories(@event.Id);

        Domain.AggregateModels.EventAggregate.Event
            .DeleteEventReasons(@event.Id);

        await _unitOfWork.CommitAsync();
    }
}
