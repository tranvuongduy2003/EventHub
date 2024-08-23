using MediatR;

namespace EventHub.Domain.SeedWork.DomainEvent;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}