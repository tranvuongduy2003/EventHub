﻿using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.EventAggregate;
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
        bool isEventExisted = await _unitOfWork.Events
            .ExistAsync(x => x.Id == notification.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        bool isFavouriteEventExisted = await _unitOfWork.FavouriteEvents
            .ExistAsync(x =>
                x.EventId == notification.EventId &&
                x.UserId == notification.UserId);
        if (isFavouriteEventExisted)
        {
            throw new BadRequestException("User has subscribed this event before");
        }

        await _unitOfWork.FavouriteEvents.CreateAsync(new FavouriteEvent
        {
            UserId = notification.UserId,
            EventId = notification.EventId,
        });

        User user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user != null)
        {
            user.NumberOfFavourites++;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}
