using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class MakeEventsPublicDomainEventHandler : IDomainEventHandler<MakeEventsPublicDomainEvent>
{
    private readonly ILogger<MakeEventsPublicDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public MakeEventsPublicDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<MakeEventsPublicDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(MakeEventsPublicDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: MakeEventsPublicDomainEventHandler");

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

        _logger.LogInformation("END: MakeEventsPublicDomainEventHandler");
    }
}