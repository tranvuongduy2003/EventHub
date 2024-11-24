using EventHub.Shared.SeedWork;

namespace EventHub.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an unauthorized request (HTTP 401) occurs.
/// </summary>
public class UnauthorizedException : HttpResponseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with an optional value.
    /// </summary>
    /// <param name="value">
    /// An optional object that provides additional information about the unauthorized request. This value is passed to the base exception class.
    /// </param>
    public UnauthorizedException(object? value = null) : base(401, value)
    {
    }
}
