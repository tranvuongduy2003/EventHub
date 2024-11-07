using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEventTicketTypesDomainEventHandler : IDomainEventHandler<DeleteEventTicketTypesDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventTicketTypesDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteEventTicketTypesDomainEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.TicketTypes
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ExecuteDeleteAsync();

        await _unitOfWork.CommitAsync();
    }
}