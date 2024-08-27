using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.RemoveFunctionFromRole;

public record RemoveFunctionFromRoleCommand(string FunctionId, Guid RoleId) : ICommand;