using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.RemoveFunctionFromRole;

public class RemoveFunctionFromRoleCommandHandler : ICommandHandler<RemoveFunctionFromRoleCommand>
{
    public RemoveFunctionFromRoleCommandHandler()
    {
    }

    public async Task Handle(RemoveFunctionFromRoleCommand request, CancellationToken cancellationToken)
    {
        await PermissionAggregateRoot.RemoveFunctionFromRole(request.FunctionId, request.RoleId);
    }
}