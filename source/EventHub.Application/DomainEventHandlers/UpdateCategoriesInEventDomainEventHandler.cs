using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Application.DomainEventHandlers;

public class UpdateCategoriesInEventDomainEventHandler : IDomainEventHandler<UpdateCategoriesInEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoriesInEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCategoriesInEventDomainEvent notification, CancellationToken cancellationToken)
    {
        IQueryable<EventCategory> deletedEventCategories = _unitOfWork.EventCategories
            .FindByCondition(x => x.EventId.Equals(notification.EventId));
        await _unitOfWork.EventCategories.DeleteList(deletedEventCategories);

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
