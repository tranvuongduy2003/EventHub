using EventHub.Application.Abstractions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        IQueryable<Event> events = _unitOfWork.CachedEvents
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
            .SetProperty(e => e.DeletedAt, (DateTime?)null), cancellationToken);

        User user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents += events.ToList().Count;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}
