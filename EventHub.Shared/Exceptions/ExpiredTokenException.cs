namespace EventHub.Shared.Exceptions;

public class ExpiredTokenException : UnauthorizedException
{
    public ExpiredTokenException() : base("expired.token")
    {
    }
}