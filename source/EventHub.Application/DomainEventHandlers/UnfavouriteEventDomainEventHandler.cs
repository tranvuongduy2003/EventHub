using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class UnfavouriteEventDomainEventHandler : IDomainEventHandler<UnfavouriteEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnfavouriteEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UnfavouriteEventDomainEvent notification, CancellationToken cancellationToken)
    {
        FavouriteEvent favouriteEvent = await _unitOfWork.FavouriteEvents
            .FindByCondition(x =>
                x.EventId == notification.EventId &&
                x.UserId == notification.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        if (favouriteEvent == null)
        {
            return;
        }

        await _unitOfWork.FavouriteEvents.Delete(favouriteEvent);

        await _unitOfWork.CommitAsync();
    }
}
