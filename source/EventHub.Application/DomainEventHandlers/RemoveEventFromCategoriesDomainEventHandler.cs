using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class RemoveEventFromCategoriesDomainEventHandler : IDomainEventHandler<RemoveEventFromCategoriesDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveEventFromCategoriesDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveEventFromCategoriesDomainEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.EventCategories
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ExecuteDeleteAsync();

        await _unitOfWork.CommitAsync();
    }
}