using Newtonsoft.Json;

namespace EventHub.Domain.Shared.HttpResponses;

public class ApiResponse
{
    public ApiResponse(int statusCode, string? message = null, object? data = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        Data = data;
    }

    public int StatusCode { get; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public object? Data { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<string> Errors { get; set; }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Success",
            201 => "Created",
            400 => "Bad request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Resource not found",
            500 => "An unhandled error occurred",
            _ => null,
        };
    }
}
