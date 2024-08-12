namespace EventHub.Shared.Exceptions;

public class InvalidTokenException : UnauthorizedException
{
    public InvalidTokenException() : base("invalid.token")
    {
    }
}