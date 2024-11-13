using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.DisableCommandInFunction;

public class DisableCommandInFunctionCommandHandler : ICommandHandler<DisableCommandInFunctionCommand>
{
    public DisableCommandInFunctionCommandHandler()
    {
    }

    public async Task Handle(DisableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        await PermissionAggregateRoot.DisableCommandInFunction(request.FunctionId, request.CommandId);
    }
}