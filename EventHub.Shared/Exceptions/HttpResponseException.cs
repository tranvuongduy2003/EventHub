namespace EventHub.Shared.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException(int statusCode, object value = null)
    {
        StatusCode = statusCode;
        Value = value;
    }

    public int StatusCode { get; }
    public object Value { get; }
}