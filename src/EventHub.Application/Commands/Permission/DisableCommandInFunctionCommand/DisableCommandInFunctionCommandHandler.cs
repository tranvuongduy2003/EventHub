using EventHub.Domain.AggregateModels.PermissionAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.DisableCommandInFunctionCommand;

public class DisableCommandInFunctionCommandHandler : IRequestHandler<DisableCommandInFunctionCommand>
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