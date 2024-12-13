using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.DomainEventHandlers;

public class FavouriteEventDomainEventHandler : IDomainEventHandler<FavouriteEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public FavouriteEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(FavouriteEventDomainEvent notification, CancellationToken cancellationToken)
    {
        bool isFavouriteEventExisted = await _unitOfWork.FavouriteEvents
            .ExistAsync(x =>
                x.EventId == notification.EventId &&
                x.UserId == notification.UserId);
        if (isFavouriteEventExisted)
        {
            return;
        }

        await _unitOfWork.FavouriteEvents.CreateAsync(new FavouriteEvent
        {
            UserId = notification.UserId,
            EventId = notification.EventId,
        });

        await _unitOfWork.CommitAsync();
    }
}
