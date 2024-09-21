using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class RemoveEventFromCategoriesDomainEventHandler : IDomainEventHandler<RemoveEventFromCategoriesDomainEvent>
{
    private readonly ILogger<RemoveEventFromCategoriesDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveEventFromCategoriesDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<RemoveEventFromCategoriesDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(RemoveEventFromCategoriesDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: RemoveEventFromCategoriesDomainEventHandler");

        await _unitOfWork.EventCategories
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ExecuteDeleteAsync();

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: RemoveEventFromCategoriesDomainEventHandler");
    }
}