using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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
        var isEventExisted = await _unitOfWork.Events
            .ExistAsync(x => x.Id.Equals(notification.EventId));
        if (!isEventExisted)
            throw new NotFoundException("Event does not exist!");

        var isFavouriteEventExisted = await _unitOfWork.FavouriteEvents
            .ExistAsync(x =>
                x.EventId.Equals(notification.EventId) &&
                x.UserId.Equals(notification.UserId));
        if (isFavouriteEventExisted)
            throw new BadRequestException("User has subscribed this event before");

        await _unitOfWork.FavouriteEvents.CreateAsync(new Domain.AggregateModels.EventAggregate.FavouriteEvent
        {
            UserId = notification.UserId,
            EventId = notification.EventId,
        });

        var user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user != null)
        {
            user.NumberOfFavourites++;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}