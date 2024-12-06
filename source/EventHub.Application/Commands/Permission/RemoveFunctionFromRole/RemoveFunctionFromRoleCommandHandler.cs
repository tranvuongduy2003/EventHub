using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.RemoveFunctionFromRole;

public class RemoveFunctionFromRoleCommandHandler : ICommandHandler<RemoveFunctionFromRoleCommand>
{
    public RemoveFunctionFromRoleCommandHandler()
    {
    }

    public async Task Handle(RemoveFunctionFromRoleCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            PermissionAggregateRoot.RemoveFunctionFromRole(request.FunctionId, request.RoleId);
        }, cancellationToken);
    }
}
