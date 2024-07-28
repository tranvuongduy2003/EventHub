namespace EventHub.Domain.Exceptions;

public class InvalidTokenException : UnauthorizedException
{
    public InvalidTokenException() : base("invalid.token")
    {
    }
}