using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class RestoreEventDomainEventHandler : IDomainEventHandler<RestoreEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public RestoreEventDomainEventHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(RestoreEventDomainEvent notification, CancellationToken cancellationToken)
    {
        var events = _unitOfWork.CachedEvents
            .FindByCondition(x =>
                x.AuthorId.Equals(notification.UserId) &&
                x.IsDeleted)
            .Join(
                notification.Events,
                _event => _event.Id,
                _id => _id,
                (_event, _id) => _event);

        await events.ExecuteUpdateAsync(setters => setters
            .SetProperty(e => e.IsDeleted, false)
            .SetProperty(e => e.DeletedAt, (DateTime?)null));

        var user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents += events.ToList().Count;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}