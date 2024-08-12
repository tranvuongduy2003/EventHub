namespace EventHub.Shared.HttpResponses;

public class ApiOkResponse : ApiResponse
{
    public ApiOkResponse()
        : base(200)
    {
    }

    public ApiOkResponse(object? data)
        : base(200, null, data)
    {
    }

    public ApiOkResponse(object? data, string? message)
        : base(200, message, data)
    {
    }
}