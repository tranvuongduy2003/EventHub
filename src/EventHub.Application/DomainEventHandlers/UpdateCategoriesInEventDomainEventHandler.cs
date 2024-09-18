using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class UpdateCategoriesInEventDomainEventHandler : IDomainEventHandler<UpdateCategoriesInEventDomainEvent>
{
    private readonly ILogger<UpdateCategoriesInEventDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoriesInEventDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<UpdateCategoriesInEventDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(UpdateCategoriesInEventDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UpdateCategoriesInEventDomainEventHandler");

        var deletedEventCategories = _unitOfWork.EventCategories
            .FindByCondition(x => x.EventId.Equals(notification.EventId));
        await _unitOfWork.EventCategories.DeleteListAsync(deletedEventCategories);

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

        _logger.LogInformation("END: UpdateCategoriesInEventDomainEventHandler");
    }
}