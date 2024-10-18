using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Persistence.Data;
using EventHub.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Idempotence;

/// <summary>
/// An idempotent domain event handler that ensures a domain event is processed only once.
/// </summary>
/// <typeparam name="TDomainEvent">
/// The type of the domain event that this handler processes. It must implement <see cref="IDomainEvent"/>.
/// </typeparam>
/// <remarks>
/// This class implements <see cref="IDomainEventHandler{TDomainEvent}"/> and provides an idempotent
/// mechanism for handling domain events. It achieves idempotency by tracking processed events in the
/// database and ensuring that each event is processed only once. It decorates an existing handler and
/// checks the database before delegating the handling to the decorated handler.
/// </remarks>
public sealed class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdempotentDomainEventHandler{TDomainEvent}"/> class.
    /// </summary>
    /// <param name="decorated">
    /// The decorated handler that actually processes the domain event. It must implement <see cref="INotificationHandler{TDomainEvent}"/>.
    /// </param>
    /// <param name="dbContext">
    /// The database context used to track processed domain events to ensure idempotency.
    /// </param>
    public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> decorated, ApplicationDbContext dbContext)
    {
        _decorated = decorated;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Handles the domain event in an idempotent manner.
    /// </summary>
    /// <param name="notification">
    /// The domain event to be handled. It must implement <see cref="IDomainEvent"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to observe and respond to cancellation requests.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        string consumer = _decorated.GetType().Name;

        // Check if the event has already been processed
        if (await _dbContext.Set<OutboxMessageConsumer>()
                .AnyAsync(outboxMessageConsumer =>
                        outboxMessageConsumer.Id == notification.Id &&
                        outboxMessageConsumer.Name == consumer,
                    cancellationToken))
        {
            return; // Event has already been processed, exit early
        }

        // Process the event
        await _decorated.Handle(notification, cancellationToken);

        // Record the event as processed
        _dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
