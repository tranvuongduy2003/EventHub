using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.DomainEventHandlers;

public class RestoreEventDomainEventHandler : IDomainEventHandler<RestoreEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public RestoreEventDomainEventHandler(IUnitOfWork unitOfWork, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task Handle(RestoreEventDomainEvent notification, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        IQueryable<Event> events = _unitOfWork.CachedEvents
            .FindByCondition(x =>
                x.AuthorId.ToString() == userId &&
                x.IsDeleted)
            .Join(
                notification.Events,
                _event => _event.Id,
                _id => _id,
                (_event, _id) => _event);

        await events.ExecuteUpdateAsync(setters => setters
            .SetProperty(e => e.IsDeleted, false)
            .SetProperty(e => e.DeletedAt, (DateTime?)null), cancellationToken);

        User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.NumberOfCreatedEvents += events.ToList().Count;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}
