﻿using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Application.DomainEventHandlers;

public class
    CreateReasonsToRegisterEventDomainEventHandler : IDomainEventHandler<CreateReasonsToRegisterEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateReasonsToRegisterEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateReasonsToRegisterEventDomainEvent notification, CancellationToken cancellationToken)
    {
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
    }
}