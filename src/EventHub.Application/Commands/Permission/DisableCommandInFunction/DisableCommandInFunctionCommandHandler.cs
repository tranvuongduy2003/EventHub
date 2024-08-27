using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.DisableCommandInFunction;

public class DisableCommandInFunctionCommandHandler : ICommandHandler<DisableCommandInFunctionCommand>
{
    private readonly ILogger<DisableCommandInFunctionCommandHandler> _logger;

    public DisableCommandInFunctionCommandHandler(ILogger<DisableCommandInFunctionCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(DisableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DisableCommandInFunctionCommandHandler");

        await PermissionAggregateRoot.DisableCommandInFunction(request.FunctionId, request.CommandId);
        
        _logger.LogInformation("END: DisableCommandInFunctionCommandHandler");
    }
}