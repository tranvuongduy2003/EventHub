using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Persistence.Data;
using EventHub.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Idempotence;

public sealed class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly ApplicationDbContext _dbContext;

    public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> decorated, ApplicationDbContext dbContext)
    {
        _decorated = decorated;
        _dbContext = dbContext;
    }
    
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        string consumer = _decorated.GetType().Name;

        if (await _dbContext.Set<OutboxMessageConsumer>()
                .AnyAsync(outboxMessageConsumer =>
                        outboxMessageConsumer.Id == notification.Id &&
                        outboxMessageConsumer.Name == consumer,
                    cancellationToken))
        {
            return;
        }

        await _decorated.Handle(notification, cancellationToken);

        _dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}