namespace EventHub.Application.SeedWork.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an invalid token is encountered, resulting in an unauthorized request (HTTP 401).
/// </summary>
public class InvalidTokenException : UnauthorizedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidTokenException"/> class with a default message indicating an invalid token.
    /// </summary>
    public InvalidTokenException() : base("invalid.token")
    {
    }
}
