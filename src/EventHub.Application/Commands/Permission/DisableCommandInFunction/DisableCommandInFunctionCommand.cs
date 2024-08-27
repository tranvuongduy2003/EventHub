using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.DisableCommandInFunction;

/// <summary>
/// Represents a command to disable a specific command within a function.
/// </summary>
/// <param name="FunctionId">
/// The unique identifier of the function where the command needs to be disabled.
/// This parameter is a string that represents the ID of the function. It is essential for locating
/// the correct function within which the command will be disabled.
/// </param>
/// <param name="CommandId">
/// The unique identifier of the command that needs to be disabled within the specified function.
/// This parameter is a string that represents the ID of the command. It is necessary to identify
/// which command should be disabled within the given function.
/// </param>
public record DisableCommandInFunctionCommand(string FunctionId, string CommandId) : ICommand;