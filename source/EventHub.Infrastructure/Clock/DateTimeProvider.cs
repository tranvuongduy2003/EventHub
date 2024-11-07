using EventHub.Abstractions;

namespace EventHub.Infrastructure.Clock;

/// <summary>
/// Provides the current UTC date and time.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IDateTimeProvider"/> interface and supplies the current
/// Coordinated Universal Time (UTC) through the <see cref="UtcNow"/> property. It encapsulates
/// the system's clock functionality, allowing for a single point of change if the method of
/// obtaining the current time needs to be altered.
/// </remarks>
public sealed class DateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> representing the current date and time in UTC.
    /// This property retrieves the current time from the system clock and is useful for
    /// obtaining a consistent time reference in UTC.
    /// </value>
    public DateTime UtcNow => DateTime.UtcNow;
}