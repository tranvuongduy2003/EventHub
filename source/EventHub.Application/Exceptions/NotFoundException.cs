using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a requested resource is not found (HTTP 404).
/// </summary>
public class NotFoundException : HttpResponseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with an optional value.
    /// </summary>
    /// <param name="value">
    /// An optional object that provides additional information about the resource that was not found. This value is passed to the base exception class.
    /// </param>
    public NotFoundException(object? value = null) : base(404, value)
    {
    }
}
