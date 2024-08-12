namespace EventHub.Shared.Exceptions;

public class UnauthorizedException : HttpResponseException
{
    public UnauthorizedException(object value = null) : base(401, value)
    {
    }
}