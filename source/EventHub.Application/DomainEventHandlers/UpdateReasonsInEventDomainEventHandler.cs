﻿using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Application.DomainEventHandlers;

public class UpdateReasonsInEventDomainEventHandler : IDomainEventHandler<UpdateReasonsInEventDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReasonsInEventDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateReasonsInEventDomainEvent notification, CancellationToken cancellationToken)
    {
        IQueryable<Reason> deletedReasons = _unitOfWork.Reasons
            .FindByCondition(x => x.EventId.Equals(notification.EventId));
        _unitOfWork.Reasons.DeleteList(deletedReasons);

        var reasons = new List<Reason>();
        foreach (string reason in notification.Reasons)
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