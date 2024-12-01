using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
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
        bool isEventExisted = await _unitOfWork.Events
            .ExistAsync(x => x.Id.Equals(notification.EventId));
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        Domain.AggregateModels.EventAggregate.FavouriteEvent favouriteEvent = await _unitOfWork.FavouriteEvents
            .FindByCondition(x =>
                x.EventId.Equals(notification.EventId) &&
                x.UserId.Equals(notification.UserId))
            .FirstOrDefaultAsync(cancellationToken);
        if (favouriteEvent == null)
        {
            throw new BadRequestException("User has not subscribed this event before");
        }

        await _unitOfWork.FavouriteEvents.Delete(favouriteEvent);

        User user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user != null)
        {
            user.NumberOfFavourites--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}
