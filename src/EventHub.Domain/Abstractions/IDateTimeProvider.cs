namespace EventHub.Domain.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}