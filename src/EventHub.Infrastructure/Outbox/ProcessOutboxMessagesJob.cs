using System.Data;
using Dapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.DomainEvent;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace EventHub.Infrastructure.Outbox;

/// <summary>
/// A job that processes outbox messages, ensuring that they are published and marked as processed.
/// </summary>
/// <remarks>
/// This class implements <see cref="IProcessOutboxMessagesJob"/> and is responsible for executing
/// the processing of outbox messages. It ensures that messages are read from the database, deserialized,
/// and published as domain events. It also handles errors and updates the status of the messages in
/// the database to reflect whether they have been successfully processed or encountered an error.
/// </remarks>
[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IProcessOutboxMessagesJob
{
    private const int BatchSize = 15;

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessOutboxMessagesJob"/> class.
    /// </summary>
    /// <param name="sqlConnectionFactory">
    /// Factory for creating SQL database connections.
    /// </param>
    /// <param name="publisher">
    /// Publisher used to publish domain events.
    /// </param>
    /// <param name="dateTimeProvider">
    /// Provider for obtaining the current UTC date and time.
    /// </param>
    /// <param name="logger">
    /// Logger for logging information and errors.
    /// </param>
    public ProcessOutboxMessagesJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
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

        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
        using IDbTransaction transaction = connection.BeginTransaction();

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        if (!outboxMessages.Any())
        {
            _logger.LogInformation("Completed processing outbox messages - no messages to process");
            return;
        }

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings)!;

                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                _logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql = $"""
                      SELECT id, content
                      FROM outbox_messages
                      WHERE processed_on_utc IS NULL
                      ORDER BY occurred_on_utc
                      LIMIT @BatchSize
                      FOR UPDATE SKIP LOCKED
                      """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
            sql,
            new { BatchSize },
            transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id";

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = _dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }

    /// <summary>
    /// Represents an outbox message response.
    /// </summary>
    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
