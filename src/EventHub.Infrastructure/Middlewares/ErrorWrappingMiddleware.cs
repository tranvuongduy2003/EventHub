using EventHub.Shared.HttpResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventHub.Infrastructure.Middlewares;

public class ErrorWrappingMiddleware
{
    private readonly ILogger<ErrorWrappingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = 500;
        }

        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse(context.Response.StatusCode);

            var json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
        }
    }
}