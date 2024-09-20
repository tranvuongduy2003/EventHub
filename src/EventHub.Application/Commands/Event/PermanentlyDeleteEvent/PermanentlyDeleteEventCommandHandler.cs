using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.PermanentlyDeleteEvent;

public class PermanentlyDeleteEventCommandHandler : ICommandHandler<PermanentlyDeleteEventCommand>
{
    private readonly ILogger<PermanentlyDeleteEventCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public PermanentlyDeleteEventCommandHandler(IUnitOfWork unitOfWork,
        ILogger<PermanentlyDeleteEventCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(PermanentlyDeleteEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: PermanentlyDeleteEventCommandHandler");

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

        _logger.LogInformation("END: PermanentlyDeleteEventCommandHandler");
    }
}