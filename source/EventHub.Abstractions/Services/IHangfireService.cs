using System.Linq.Expressions;

namespace EventHub.Abstractions.Services;

/// <summary>
/// Defines a contract for interacting with Hangfire to manage background jobs.
/// </summary>
public interface IHangfireService
{
    #region Continuos Jobs

    /// <summary>
    /// Continues a queued job with a specified function call.
    /// </summary>
    /// <param name="parentJobId">
    /// The unique identifier of the parent job that should continue with the specified function call.
    /// </param>
    /// <param name="functionCall">
    /// An expression representing the function call to be executed as a continuation of the parent job.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created job.
    /// </returns>
    string ContinueQueueWith(string parentJobId, Expression<Action> functionCall);

    #endregion

    /// <summary>
    /// Deletes a job identified by its unique identifier.
    /// </summary>
    /// <param name="jobId">
    /// The unique identifier of the job to be deleted.
    /// </param>
    /// <returns>
    /// <c>true</c> if the job was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    bool Delete(string jobId);

    /// <summary>
    /// Requeues a job identified by its unique identifier.
    /// </summary>
    /// <param name="jobId">
    /// The unique identifier of the job to be requeued.
    /// </param>
    /// <returns>
    /// <c>true</c> if the job was successfully requeued; otherwise, <c>false</c>.
    /// </returns>
    bool Requeue(string jobId);

    #region Fire and Forget

    /// <summary>
    /// Enqueues a fire-and-forget job with the specified function call.
    /// </summary>
    /// <param name="functionCall">
    /// An expression representing the function call to be executed as a fire-and-forget job.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created job.
    /// </returns>
    string Enqueue(Expression<Action> functionCall);

    /// <summary>
    /// Enqueues a fire-and-forget job with a specified function call that has a generic parameter.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the parameter for the function call.
    /// </typeparam>
    /// <param name="functionCall">
    /// An expression representing the function call with a generic parameter to be executed as a fire-and-forget job.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created job.
    /// </returns>
    string Enqueue<T>(Expression<Action<T>> functionCall);

    #endregion

    #region Delayed Jobs

    /// <summary>
    /// Schedules a delayed job with the specified function call and delay.
    /// </summary>
    /// <param name="functionCall">
    /// An expression representing the function call to be executed as a delayed job.
    /// </param>
    /// <param name="delay">
    /// The amount of time to wait before executing the job.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created job.
    /// </returns>
    string Schedule(Expression<Action> functionCall, TimeSpan delay);

    /// <summary>
    /// Schedules a delayed job with a specified function call and delay for a generic parameter.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the parameter for the function call.
    /// </typeparam>
    /// <param name="functionCall">
    /// An expression representing the function call with a generic parameter to be executed as a delayed job.
    /// </param>
    /// <param name="delay">
    /// The amount of time to wait before executing the job.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created job.
    /// </returns>
    string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay);

    /// <summary>
    /// Schedules a delayed job with the specified function call and absolute enqueue time.
    /// </summary>
    /// <param name="functionCall">
    /// An expression representing the function call to be executed as a delayed job.
    /// </param>
    /// <param name="enqueueAt">
    /// The absolute date and time at which the job should be enqueued.
    /// </param>
    /// <returns>
    /// The unique identifier of the newly created job.
    /// </returns>
    string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt);

    #endregion
}
