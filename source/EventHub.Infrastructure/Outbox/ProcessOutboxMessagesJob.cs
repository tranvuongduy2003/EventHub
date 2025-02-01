using System.Data;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace EventHub.Infrastructure.Outbox;

/// <summary>
/// A job that processes outbox messages, ensuring that they are published and marked as processed.
/// </summary>
/// <remarks>
/// This class implements <see cref="IJob"/> and is responsible for executing
/// the processing of outbox messages. It ensures that messages are read from the database, deserialized,
/// and published as domain events. It also handles errors and updates the status of the messages in
/// the database to reflect whether they have been successfully processed or encountered an error.
/// </remarks>
[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private const int BatchSize = 10;

    private readonly ILogger<ProcessOutboxMessagesJob> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;


    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessOutboxMessagesJob"/> class.
    /// </summary>
    /// <param name="dbContext">
    /// ApplicationDbContext.
    /// </param>
    /// <param name="publisher">
    /// Publisher used to publish domain events.
    /// </param>
    /// <param name="logger">
    /// Logger for logging information and errors.
    /// </param>
    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IPublisher publisher,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    /// <summary>
    /// Executes the job to process outbox messages.
    /// </summary>
    /// <param name="context">
    /// The context for job execution, providing information such as cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");

        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        if (!messages.Any())
        {
            _logger.LogInformation("Completed processing outbox messages - no messages to process");
            return;
        }

        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            await _publisher.Publish(domainEvent, context.CancellationToken);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Completed processing outbox messages");
    }
}
