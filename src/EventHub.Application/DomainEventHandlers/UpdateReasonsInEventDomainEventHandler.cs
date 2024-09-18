using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class UpdateReasonsInEventDomainEventHandler : IDomainEventHandler<UpdateReasonsInEventDomainEvent>
{
    private readonly ILogger<UpdateReasonsInEventDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReasonsInEventDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<UpdateReasonsInEventDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(UpdateReasonsInEventDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UpdateReasonsInEventDomainEventHandler");

        var deletedReasons = _unitOfWork.Reasons
            .FindByCondition(x => x.EventId.Equals(notification.EventId));
        await _unitOfWork.Reasons.DeleteListAsync(deletedReasons);

        var reasons = new List<Reason>();
        foreach (var reason in notification.Reasons)
        {
            reasons.Add(new Reason()
            {
                EventId = notification.EventId,
                Name = reason
            });
        }

        await _unitOfWork.Reasons.CreateListAsync(reasons);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: UpdateReasonsInEventDomainEventHandler");
    }
}