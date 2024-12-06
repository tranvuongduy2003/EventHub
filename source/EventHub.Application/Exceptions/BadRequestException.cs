using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a bad request (HTTP 400) occurs.
/// </summary>
public class BadRequestException : HttpResponseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with an optional value.
    /// </summary>
    /// <param name="value">
    /// An optional object that provides additional information about the bad request. This value is passed to the base exception class.
    /// </param>
    public BadRequestException(object? value = null) : base(400, value)
    {
    }
}
