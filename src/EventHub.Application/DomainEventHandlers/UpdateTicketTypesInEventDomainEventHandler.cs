using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.Extensions.Logging;

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
        foreach (var ticketType in notification.TicketTypes)
        {
            if (ticketType.Id != null)
            {
                var ticketTypeEntity = await _unitOfWork.TicketTypes.GetByIdAsync(ticketType.Id ?? new Guid());
                ticketTypeEntity.Name = ticketType.Name;
                ticketTypeEntity.Quantity = ticketType.Quantity;
                ticketTypeEntity.Price = ticketType.Price;
                await _unitOfWork.TicketTypes.UpdateAsync(ticketTypeEntity);
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