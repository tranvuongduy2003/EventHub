using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.DomainEventHandlers;

public class MakeEventsPrivateDomainEventHandler : IDomainEventHandler<MakeEventsPrivateDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<User> _signInManager;

    public MakeEventsPrivateDomainEventHandler(IUnitOfWork unitOfWork, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public async Task Handle(MakeEventsPrivateDomainEvent notification, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";
        
        IQueryable<Event> events = _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.ToString() == userId)
            .Join(
                notification.Events,
                _event => _event.Id,
                _id => _id, (_event, _id) => _event);

        await events.ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsPrivate, true), cancellationToken);

        await _unitOfWork.CommitAsync();
    }
}
