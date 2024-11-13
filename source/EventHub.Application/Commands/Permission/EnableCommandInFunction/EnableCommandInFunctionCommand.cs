using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.EnableCommandInFunction;

/// <summary>
/// Represents a command to enable a specific command within a function.
/// </summary>
/// <param name="FunctionId">
/// The unique identifier of the function where the command needs to be enabled.
/// This parameter is a string that identifies the function within which the command will be enabled.
/// </param>
/// <param name="CommandId">
/// The unique identifier of the command that needs to be enabled within the specified function.
/// This parameter is a string that identifies the command that should be enabled in the given function.
/// </param>
public record EnableCommandInFunctionCommand(string FunctionId, string CommandId) : ICommand;