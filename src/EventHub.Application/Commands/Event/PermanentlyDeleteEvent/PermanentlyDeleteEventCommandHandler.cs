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
        var @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
            throw new NotFoundException("Event does not exist!");

        await _unitOfWork.Events.DeleteAsync(@event);

        await Domain.AggregateModels.EventAggregate.Event
            .DeleteEventSubImages(@event.Id);

        await Domain.AggregateModels.EventAggregate.Event
            .DeleteEmailContent(@event.Id);

        await Domain.AggregateModels.EventAggregate.Event
            .DeleteEventTicketTypes(@event.Id);

        await Domain.AggregateModels.EventAggregate.Event
            .RemoveEventFromCategories(@event.Id);

        await Domain.AggregateModels.EventAggregate.Event
            .DeleteEventReasons(@event.Id);

        await _unitOfWork.CommitAsync();
    }
}