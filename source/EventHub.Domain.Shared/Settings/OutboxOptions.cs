namespace EventHub.Domain.Shared.Settings;

public sealed class OutboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}
