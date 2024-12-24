using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class UnfavouriteEventDomainEventHandler : IDomainEventHandler<UnfavouriteEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public UnfavouriteEventDomainEventHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(UnfavouriteEventDomainEvent notification, CancellationToken cancellationToken)
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

        @event.NumberOfFavourites = (@event.NumberOfFavourites ?? 0) + 1;
        await _unitOfWork.CachedEvents.Update(@event);

        user.NumberOfFavourites = (user.NumberOfFavourites ?? 0) + 1;
        await _userManager.UpdateAsync(user);

        await _unitOfWork.CommitAsync();
    }
}
