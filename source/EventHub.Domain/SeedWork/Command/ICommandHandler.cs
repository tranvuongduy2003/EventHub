using MediatR;

namespace EventHub.Domain.SeedWork.Command;

/// <summary>
/// Defines a handler for processing commands that return a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command to handle. Must implement <see cref="ICommand{TCommandResponse}"/>.</typeparam>
/// <typeparam name="TCommandResponse">The type of the response returned by the command handler.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequestHandler{TCommand, TCommandResponse}"/>, which means it is used
/// to handle commands of type <typeparamref name="TCommand"/> that produce a result of type <typeparamref name="TCommandResponse"/>.
/// Implementations of this interface are responsible for executing the command and returning a result based on the command's logic.
/// </remarks>
public interface ICommandHandler<TCommand, TCommandResponse> : IRequestHandler<TCommand, TCommandResponse>
    where TCommand : ICommand<TCommandResponse>
{
}

/// <summary>
/// Defines a handler for processing commands that do not return a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command to handle. Must implement <see cref="ICommand"/>.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequestHandler{TCommand}"/>, which indicates it is used
/// to handle commands of type <typeparamref name="TCommand"/> that do not return a result. Implementations of this
/// interface are responsible for executing the command's logic without producing a result.
/// </remarks>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}