using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace EventHub.Infrastructure.Persistence.Interceptors;

/// <summary>
/// An EF Core interceptor that converts domain events into outbox messages before saving changes.
/// </summary>
/// <remarks>
/// This class extends <see cref="SaveChangesInterceptor"/> to intercept the `SavingChangesAsync` event
/// and convert domain events from aggregate roots into outbox messages. These outbox messages are then
/// added to the `OutboxMessage` table to ensure that domain events can be processed and published asynchronously.
/// </remarks>
public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts the `SavingChangesAsync` event to convert domain events into outbox messages.
    /// </summary>
    /// <param name="eventData">
    /// The data related to the EF Core event, including the <see cref="DbContext"/> instance.
    /// </param>
    /// <param name="result">
    /// The result of the interception up to this point.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to observe and respond to cancellation requests.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{InterceptionResult{T}}"/> representing the asynchronous operation.
    /// </returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var outboxMessages = dbContext
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = aggregateRoot.GetDomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
