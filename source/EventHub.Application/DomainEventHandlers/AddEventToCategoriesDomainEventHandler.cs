using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Application.DomainEventHandlers;

public class AddEventToCategoriesDomainEventHandler : IDomainEventHandler<AddEventToCategoriesDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddEventToCategoriesDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddEventToCategoriesDomainEvent notification, CancellationToken cancellationToken)
    {
        var eventCategories = new List<EventCategory>();

        foreach (Guid categoryId in notification.Categories)
        {
            eventCategories.Add(new EventCategory()
            {
                CategoryId = categoryId,
                EventId = notification.EventId
            });
        }

        await _unitOfWork.EventCategories.CreateListAsync(eventCategories);
        await _unitOfWork.CommitAsync();
    }
}
