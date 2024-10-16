using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Behaviors;

/// <summary>
/// Represents a behavior in the MediatR pipeline that logs the start and end of request handling.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
/// <typeparam name="TResponse">The type of the response from the handler.</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">
    /// The logger used to record information about the request handling process.
    /// </param>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the logging behavior before and after the request is processed.
    /// </summary>
    /// <param name="request">
    /// The request object being handled.
    /// </param>
    /// <param name="next">
    /// A delegate representing the next action in the pipeline.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResponse}"/> representing the result of the next action in the pipeline.
    /// </returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN: {typeof(TRequest).Name}Handler");

        var response = await next();

        _logger.LogInformation($"END: {typeof(TRequest).Name}Handler");

        return response;
    }
}