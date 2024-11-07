using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.AddFunctionToRole;

public record AddFunctionToRoleCommand(string FunctionId, Guid RoleId) : ICommand;