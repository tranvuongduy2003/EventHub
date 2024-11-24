using MediatR;

namespace EventHub.Domain.SeedWork.DomainEvent;

/// <summary>
/// Defines a handler for processing domain events.
/// </summary>
/// <typeparam name="TEvent">
/// The type of the domain event that this handler will process. It must implement <see cref="IDomainEvent"/>.
/// </typeparam>
/// <remarks>
/// This interface extends <see cref="INotificationHandler{TEvent}"/> and is used to handle domain events
/// within an event-driven system. Implementations of this interface should provide the logic for
/// handling specific domain events and performing necessary actions when those events occur.
/// </remarks>
public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
