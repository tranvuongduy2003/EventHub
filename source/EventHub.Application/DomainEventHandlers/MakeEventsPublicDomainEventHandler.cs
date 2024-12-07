using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.EntityFrameworkCore;

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
        IQueryable<Event> events = _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.Equals(notification.UserId))
            .Join(
                notification.Events,
                _event => _event.Id,
                _id => _id, (_event, _id) => _event);

        await events.ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsPrivate, false), cancellationToken);

        await _unitOfWork.CommitAsync();
    }
}
