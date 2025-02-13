using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.UpdateUserRoles;

public class UpdateUserRolesCommand : ICommand
{
    public UpdateUserRolesCommand(Guid userId, UpdateUserRolesDto request)
        => (UserId, Roles) = (userId, request.Roles);

    public Guid UserId { get; set; }

    public List<string> Roles { get; set; }
}
