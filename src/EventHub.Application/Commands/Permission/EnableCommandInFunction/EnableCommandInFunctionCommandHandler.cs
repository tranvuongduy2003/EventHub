using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.EnableCommandInFunction;

public class EnableCommandInFunctionCommandHandler : ICommandHandler<EnableCommandInFunction.EnableCommandInFunctionCommand>
{
    private readonly ILogger<EnableCommandInFunctionCommandHandler> _logger;

    public EnableCommandInFunctionCommandHandler(ILogger<EnableCommandInFunctionCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(EnableCommandInFunction.EnableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: EnableCommandInFunctionCommandHandler");

        await PermissionAggregateRoot.EnableCommandInFunction(request.FunctionId, request.CommandId);
        
        _logger.LogInformation("END: EnableCommandInFunctionCommandHandler");
    }
}