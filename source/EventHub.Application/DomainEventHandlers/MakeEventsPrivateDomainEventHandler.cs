using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class MakeEventsPrivateDomainEventHandler : IDomainEventHandler<MakeEventsPrivateDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public MakeEventsPrivateDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(MakeEventsPrivateDomainEvent notification, CancellationToken cancellationToken)
    {
        var events = _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.Equals(notification.UserId))
            .Join(
                notification.Events,
                _event => _event.Id,
                _id => _id, (_event, _id) => _event);

        await events
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(e => e.IsPrivate, true));

        await _unitOfWork.CommitAsync();
    }
}