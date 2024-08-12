namespace EventHub.Shared.Exceptions;

public class NotFoundException : HttpResponseException
{
    public NotFoundException(object value = null) : base(404, value)
    {
    }
}