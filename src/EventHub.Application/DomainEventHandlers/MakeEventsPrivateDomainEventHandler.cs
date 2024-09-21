using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class MakeEventsPrivateDomainEventHandler : IDomainEventHandler<MakeEventsPrivateDomainEvent>
{
    private readonly ILogger<MakeEventsPrivateDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public MakeEventsPrivateDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<MakeEventsPrivateDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(MakeEventsPrivateDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: MakeEventsPrivateDomainEventHandler");

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

        _logger.LogInformation("END: MakeEventsPrivateDomainEventHandler");
    }
}