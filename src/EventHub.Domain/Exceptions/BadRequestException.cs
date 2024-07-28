namespace EventHub.Domain.Exceptions;

public class BadRequestException : HttpResponseException
{
    public BadRequestException(object value = null) : base(400, value)
    {
    }
}