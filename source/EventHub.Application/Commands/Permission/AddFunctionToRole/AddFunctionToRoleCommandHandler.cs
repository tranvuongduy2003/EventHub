using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.AddFunctionToRole;

public class AddFunctionToRoleCommandHandler : ICommandHandler<AddFunctionToRoleCommand>
{
    public AddFunctionToRoleCommandHandler()
    {
    }

    public async Task Handle(AddFunctionToRoleCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            PermissionAggregateRoot.AddFunctionToRole(request.FunctionId, request.RoleId);
        }, cancellationToken);
    }
}
