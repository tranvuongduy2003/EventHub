namespace EventHub.Shared.Exceptions;

/// <summary>
/// Represents an exception that can be thrown to return a specific HTTP response status code and optional value.
/// </summary>
/// <remarks>
/// This class is used to signal that an HTTP response should be generated with a specific status code and an optional value.
/// It is useful for handling and customizing HTTP responses in a web application.
/// </remarks>
public class HttpResponseException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseException"/> class with a specified status code and optional value.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to be returned.</param>
    /// <param name="value">An optional value to include in the response. This can be any object, such as an error message or additional data.</param>
    public HttpResponseException(int statusCode, object value = null)
    {
        StatusCode = statusCode;
        Value = value;
    }

    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    /// <value>
    /// An integer representing the HTTP status code. This is used to indicate the type of error or response.
    /// </value>
    public int StatusCode { get; }

    /// <summary>
    /// Gets the optional value associated with the exception.
    /// </summary>
    /// <value>
    /// An object representing additional data or an error message to include in the response. This can be null if no value is provided.
    /// </value>
    public object Value { get; }
}
