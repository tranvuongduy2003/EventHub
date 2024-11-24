using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.DTOs.Event;

namespace EventHub.Application.DomainEventHandlers;

public class UpdateTicketTypesInEventDomainEventHandler : IDomainEventHandler<UpdateTicketTypesInEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketTypesInEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTicketTypesInEventDomainEvent notification, CancellationToken cancellationToken)
    {
        var createdTicketTypes = new List<TicketType>();
        foreach (UpdateTicketTypeDto ticketType in notification.TicketTypes)
        {
            if (ticketType.Id != null)
            {
                TicketType ticketTypeEntity = await _unitOfWork.TicketTypes.GetByIdAsync((Guid)ticketType.Id);
                ticketTypeEntity.Name = ticketType.Name;
                ticketTypeEntity.Quantity = ticketType.Quantity;
                ticketTypeEntity.Price = ticketType.Price;
                _unitOfWork.TicketTypes.Update(ticketTypeEntity);
            }
            else
            {
                createdTicketTypes.Add(new TicketType()
                {
                    EventId = notification.EventId,
                    Name = ticketType.Name,
                    Price = ticketType.Price,
                    Quantity = ticketType.Quantity
                });
            }
        }

        if (createdTicketTypes.Any())
        {
            await _unitOfWork.TicketTypes.CreateListAsync(createdTicketTypes);
        }

        await _unitOfWork.CommitAsync();
    }
}
