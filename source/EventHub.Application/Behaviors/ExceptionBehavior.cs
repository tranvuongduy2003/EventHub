using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Behaviors;

/// <summary>
/// Represents a behavior in the MediatR pipeline that handles unhandled exceptions during request processing.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
/// <typeparam name="TResponse">The type of the response from the handler.</typeparam>
public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="logger">
    /// The logger used to record errors when an unhandled exception occurs during the request handling process.
    /// </param>
    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the unhandled exception behavior when processing the request.
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
    /// A <see cref="Task{TResponse}"/> representing the result of the next action in the pipeline, or throws an exception if an error occurs.
    /// </returns>
    /// <exception cref="Exception">
    /// Throws the caught exception after logging it.
    /// </exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            string requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
