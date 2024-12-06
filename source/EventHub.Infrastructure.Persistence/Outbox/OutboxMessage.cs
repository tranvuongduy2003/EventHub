namespace EventHub.Infrastructure.Persistence.Outbox;

/// <summary>
/// Represents an outbox message for storing domain events in the database.
/// </summary>
/// <remarks>
/// The <see cref="OutboxMessage"/> class is used to persist domain events in an outbox table. This allows
/// domain events to be reliably stored and processed asynchronously. It contains information about the
/// event, including its type, content, and processing status.
/// </remarks>
public sealed class OutboxMessage
{
    /// <summary>
    /// Gets or sets the unique identifier of the outbox message.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the unique identifier for the outbox message.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the domain event represented by this outbox message.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the type of the domain event.
    /// </value>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JSON-serialized content of the domain event.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the JSON-serialized representation of the domain event.
    /// </value>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC date and time when the domain event occurred.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> representing the UTC date and time when the event occurred.
    /// </value>
    public DateTime OccurredOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the outbox message was processed.
    /// </summary>
    /// <value>
    /// A nullable <see cref="DateTime"/> representing the UTC date and time when the message was processed,
    /// or null if the message has not been processed yet.
    /// </value>
    public DateTime? ProcessedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets any error message that occurred during the processing of the outbox message.
    /// </summary>
    /// <value>
    /// A nullable <see cref="string"/> containing an error message if processing failed, or null if no error occurred.
    /// </value>
    public string? Error { get; set; }
}
