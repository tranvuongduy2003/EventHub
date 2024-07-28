using Newtonsoft.Json;

namespace EventHub.Domain.HttpResponses;

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
        switch (statusCode)
        {
            case 200:
                return "Success";

            case 201:
                return "Created";

            case 400:
                return "Bad request";

            case 401:
                return "Unauthorized";

            case 403:
                return "Forbidden";

            case 404:
                return "Resource not found";

            case 500:
                return "An unhandled error occurred";

            default:
                return null;
        }
    }
}