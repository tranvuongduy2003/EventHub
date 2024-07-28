namespace EventHub.Domain.Exceptions;

public class ExpiredTokenException : UnauthorizedException
{
    public ExpiredTokenException() : base("expired.token")
    {
    }
}