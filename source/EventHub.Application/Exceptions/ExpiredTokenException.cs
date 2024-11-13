namespace EventHub.Application.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an expired token is encountered, resulting in an unauthorized request (HTTP 401).
/// </summary>
public class ExpiredTokenException : UnauthorizedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpiredTokenException"/> class with a default message indicating an expired token.
    /// </summary>
    public ExpiredTokenException() : base("expired.token")
    {
    }
}