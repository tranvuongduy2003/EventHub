using Quartz;

namespace EventHub.Application.SeedWork.Abstractions;

/// <summary>
/// Represents a job for processing outbox messages.
/// </summary>
/// <remarks>
/// This interface extends <see cref="IJob"/> and is intended for use with a job scheduling
/// or background processing system. Implementations of this interface should define the logic
/// for handling messages stored in an outbox, such as processing, sending, or archiving them.
/// </remarks>
public interface IProcessOutboxMessagesJob : IJob
{
}
