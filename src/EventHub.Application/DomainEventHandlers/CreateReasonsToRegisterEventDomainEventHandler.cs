using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class
    CreateReasonsToRegisterEventDomainEventHandler : IDomainEventHandler<CreateReasonsToRegisterEventDomainEvent>
{
    private readonly ILogger<CreateReasonsToRegisterEventDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReasonsToRegisterEventDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<CreateReasonsToRegisterEventDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(CreateReasonsToRegisterEventDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateReasonsToRegisterEventDomainEventHandler");

        var reasons = new List<Reason>();
        foreach (var reason in notification.Reasons)
        {
            reasons.Add(new Reason()
            {
                EventId = notification.EventId,
                Name = reason
            });
        }

        await _unitOfWork.Reasons.CreateListAsync(reasons);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: CreateReasonsToRegisterEventDomainEventHandler");
    }
}