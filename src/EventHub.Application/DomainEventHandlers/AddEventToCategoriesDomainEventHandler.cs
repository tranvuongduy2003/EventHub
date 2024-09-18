using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class AddEventToCategoriesDomainEventHandler : IDomainEventHandler<AddEventToCategoriesDomainEvent>
{
    private readonly ILogger<AddEventToCategoriesDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddEventToCategoriesDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<AddEventToCategoriesDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(AddEventToCategoriesDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: AddEventToCategoriesDomainEventHandler");

        var eventCategories = new List<EventCategory>();
        foreach (var categoryId in notification.Categories)
        {
            eventCategories.Add(new EventCategory()
            {
                CategoryId = categoryId,
                EventId = notification.EventId
            });
        }

        await _unitOfWork.EventCategories.CreateListAsync(eventCategories);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: AddEventToCategoriesDomainEventHandler");
    }
}