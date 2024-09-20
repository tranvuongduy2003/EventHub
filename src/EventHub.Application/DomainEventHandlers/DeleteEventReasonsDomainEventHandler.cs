using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEventReasonsDomainEventHandler : IDomainEventHandler<DeleteEventReasonsDomainEvent>
{
    private readonly ILogger<DeleteEventReasonsDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventReasonsDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteEventReasonsDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteEventReasonsDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteEventReasonsDomainEventHandler");

        await _unitOfWork.Reasons
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ExecuteDeleteAsync();

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DeleteEventReasonsDomainEventHandler");
    }
}