using MediatR;

namespace EventHub.Domain.SeedWork.Query;

/// <summary>
/// Represents a query that returns a result of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the result that the query returns.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequest{T}"/>, which means it is used to encapsulate a request
/// that will return a result of type <typeparamref name="T"/>. Implementations of this interface are typically
/// used in query-based architectures where the intention is to retrieve data without modifying the state.
/// </remarks>
public interface IQuery<T> : IRequest<T>;

/// <summary>
/// Represents a query that no returns.
/// </summary>
public interface IQuery : IRequest;