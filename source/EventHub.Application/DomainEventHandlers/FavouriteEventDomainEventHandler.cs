using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.DomainEventHandlers;

public class FavouriteEventDomainEventHandler : IDomainEventHandler<FavouriteEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public FavouriteEventDomainEventHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(FavouriteEventDomainEvent notification, CancellationToken cancellationToken)
    {
        Event @event = await _unitOfWork.Events.GetByIdAsync(notification.EventId);
        if (@event == null)
        {
            return;
        }
        User user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user == null)
        {
            return;
        }

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

        @event.NumberOfFavourites = (@event.NumberOfFavourites ?? 0) + 1;
        await _unitOfWork.CachedEvents.Update(@event);
        
        user.NumberOfFavourites = (user.NumberOfFavourites ?? 0) + 1;
        await _userManager.UpdateAsync(user);

        await _unitOfWork.CommitAsync();
    }
}
