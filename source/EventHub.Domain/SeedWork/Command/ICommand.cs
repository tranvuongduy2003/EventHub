using MediatR;

namespace EventHub.Domain.SeedWork.Command;

/// <summary>
/// Represents a command that performs an action and returns a result of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the result that the command returns.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequest{T}"/>, indicating that it encapsulates a command
/// which executes an operation and returns a result of type <typeparamref name="T"/>. Commands are
/// typically used to perform actions that may modify the state of the system and are intended for
/// operations rather than queries.
/// </remarks>
public interface ICommand<out T> : IRequest<T>;

/// <summary>
/// Represents a command that performs an action and no returns.
/// </summary>
public interface ICommand : IRequest;
