using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.DisableCommandInFunction;

public class DisableCommandInFunctionCommandHandler : ICommandHandler<DisableCommandInFunctionCommand>
{
    public DisableCommandInFunctionCommandHandler()
    {
    }

    public async Task Handle(DisableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            PermissionAggregateRoot.DisableCommandInFunction(request.FunctionId, request.CommandId);
        }, cancellationToken);
    }
}
