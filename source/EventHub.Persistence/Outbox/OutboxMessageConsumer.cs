namespace EventHub.Persistence.Outbox;

/// <summary>
/// Represents a record of an outbox message consumer for tracking the processing of outbox messages.
/// </summary>
/// <remarks>
/// The <see cref="OutboxMessageConsumer"/> class is used to record which consumers (handlers) have processed
/// specific outbox messages. This helps in ensuring that each outbox message is processed only once and
/// prevents duplicate processing by tracking the consumer who has handled a particular message.
/// </remarks>
public sealed class OutboxMessageConsumer
{
    /// <summary>
    /// Gets or sets the unique identifier of the outbox message consumer record.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier for the outbox message consumer record.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the consumer that has processed the outbox message.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the name of the consumer (e.g., handler or service) that has processed
    /// the outbox message.
    /// </value>
    public string Name { get; set; } = string.Empty;
}
