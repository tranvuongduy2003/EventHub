using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.HttpResponses;
using Newtonsoft.Json;

namespace EventHub.Presentation.Middlewares;

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
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            var response = new ApiNotFoundResponse(ex.Message);
            string json = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(json);
        }
        catch (BadRequestException ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            var response = new ApiBadRequestResponse(ex.Message);
            string json = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(json);
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;
            var response = new ApiUnauthorizedResponse(ex.Message);
            string json = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);

            context.Response.StatusCode = 500;
        }

        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse(context.Response.StatusCode);

            string json = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(json);
        }
    }
}
