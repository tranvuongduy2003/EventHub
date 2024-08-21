using MediatR;

namespace EventHub.Application.Commands.Function.DeleteFunction;

/// <summary>
/// Represents a command to delete a function by its unique identifier.
/// </summary>
/// <remarks>
/// This command is used to request the deletion of a function specified by its unique identifier.
/// </remarks>
public record DeleteFunctionCommand(string FunctionId) : IRequest;