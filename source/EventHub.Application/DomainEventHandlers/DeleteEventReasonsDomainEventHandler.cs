using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEventReasonsDomainEventHandler : IDomainEventHandler<DeleteEventReasonsDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventReasonsDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteEventReasonsDomainEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.Reasons
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ExecuteDeleteAsync(cancellationToken);

        await _unitOfWork.CommitAsync();
    }
}
