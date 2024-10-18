using System.Linq.Expressions;
using EventHub.Abstractions;
using Hangfire;

namespace EventHub.Infrastructure.Services;

/// <summary>
/// Provides a service for interacting with Hangfire, a framework for background job processing.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IHangfireService"/> interface but does not include any specific functionality or members as currently defined.
/// </remarks>
public class HangfireService : IHangfireService
{
    public string Enqueue(Expression<Action> functionCall)
    {
        return BackgroundJob.Enqueue(functionCall);
    }

    public string Enqueue<T>(Expression<Action<T>> functionCall)
    {
        return BackgroundJob.Enqueue(functionCall);
    }

    public string Schedule(Expression<Action> functionCall, TimeSpan delay)
    {
        return BackgroundJob.Schedule(functionCall, delay);
    }

    public string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay)
    {
        return BackgroundJob.Schedule(functionCall, delay);
    }

    public string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt)
    {
        return BackgroundJob.Schedule(functionCall, enqueueAt);
    }

    public string ContinueQueueWith(string parentJobId, Expression<Action> functionCall)
    {
        return BackgroundJob.ContinueJobWith(parentJobId, functionCall);
    }

    public bool Delete(string jobId)
    {
        return BackgroundJob.Delete(jobId);
    }

    public bool Requeue(string jobId)
    {
        return BackgroundJob.Requeue(jobId);
    }
}