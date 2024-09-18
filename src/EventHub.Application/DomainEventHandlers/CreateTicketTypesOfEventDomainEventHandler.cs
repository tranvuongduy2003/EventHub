using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class CreateTicketTypesOfEventDomainEventHandler : IDomainEventHandler<CreateTicketTypesOfEventDomainEvent>
{
    private readonly ILogger<CreateTicketTypesOfEventDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketTypesOfEventDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<CreateTicketTypesOfEventDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(CreateTicketTypesOfEventDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateTicketTypesOfEventDomainEventHandler");

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

        _logger.LogInformation("END: CreateTicketTypesOfEventDomainEventHandler");
    }
}