using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class MakeEventsPublicDomainEventHandler : IDomainEventHandler<MakeEventsPublicDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public MakeEventsPublicDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(MakeEventsPublicDomainEvent notification, CancellationToken cancellationToken)
    {
        var events = _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.Equals(notification.UserId))
            .Join(
                notification.Events,
                _event => _event.Id,
                _id => _id, (_event, _id) => _event);

        await events
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(e => e.IsPrivate, false));

        await _unitOfWork.CommitAsync();
    }
}