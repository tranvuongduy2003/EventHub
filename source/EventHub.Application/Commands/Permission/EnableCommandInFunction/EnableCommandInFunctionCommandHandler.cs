using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Permission.EnableCommandInFunction;

public class EnableCommandInFunctionCommandHandler : ICommandHandler<EnableCommandInFunctionCommand>
{
    public EnableCommandInFunctionCommandHandler()
    {
    }

    public async Task Handle(EnableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            PermissionAggregateRoot.EnableCommandInFunction(request.FunctionId, request.CommandId);
        }, cancellationToken);
    }
}
