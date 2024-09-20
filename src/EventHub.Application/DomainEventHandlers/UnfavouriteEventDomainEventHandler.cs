using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class UnfavouriteEventDomainEventHandler : IDomainEventHandler<UnfavouriteEventDomainEvent>
{
    private readonly ILogger<UnfavouriteEventDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public UnfavouriteEventDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<UnfavouriteEventDomainEventHandler> logger, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task Handle(UnfavouriteEventDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UnfavouriteEventDomainEventHandler");

        var isEventExisted = await _unitOfWork.Events
            .ExistAsync(x => x.Id.Equals(notification.EventId));
        if (!isEventExisted)
            throw new NotFoundException("Event does not exist!");

        var favouriteEvent = await _unitOfWork.FavouriteEvents
            .FindByCondition(x =>
                x.EventId.Equals(notification.EventId) &&
                x.UserId.Equals(notification.UserId))
            .FirstOrDefaultAsync();
        if (favouriteEvent == null)
            throw new BadRequestException("User has not subscribed this event before");

        await _unitOfWork.FavouriteEvents.DeleteAsync(favouriteEvent);

        var user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user != null)
        {
            user.NumberOfFavourites--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: UnfavouriteEventDomainEventHandler");
    }
}