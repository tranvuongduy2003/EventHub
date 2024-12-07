namespace EventHub.Application.SeedWork.Abstractions;

/// <summary>
/// Provides a way to obtain the current UTC date and time.
/// </summary>
/// <remarks>
/// Implementations of this interface should provide the current Coordinated Universal Time (UTC)
/// to allow for time-related operations that are consistent regardless of the local time zone.
/// </remarks>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> representing the current date and time in UTC.
    /// </value>
    DateTime UtcNow { get; }
}
