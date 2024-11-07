using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Application.DomainEventHandlers;

public class CreateTicketTypesOfEventDomainEventHandler : IDomainEventHandler<CreateTicketTypesOfEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketTypesOfEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateTicketTypesOfEventDomainEvent notification, CancellationToken cancellationToken)
    {
        var ticketTypes = new List<TicketType>();
        foreach (var ticketType in notification.TicketTypes)
        {
            ticketTypes.Add(new TicketType()
            {
                EventId = notification.EventId,
                Name = ticketType.Name,
                Price = ticketType.Price,
                Quantity = ticketType.Quantity
            });
        }

        await _unitOfWork.TicketTypes.CreateListAsync(ticketTypes);
        await _unitOfWork.CommitAsync();
    }
}