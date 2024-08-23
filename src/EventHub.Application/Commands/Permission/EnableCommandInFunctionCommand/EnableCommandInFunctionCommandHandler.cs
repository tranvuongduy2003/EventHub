using EventHub.Domain.AggregateModels.PermissionAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.EnableCommandInFunctionCommand;

public class EnableCommandInFunctionCommandHandler : IRequestHandler<EnableCommandInFunctionCommand>
{
    private readonly ILogger<EnableCommandInFunctionCommandHandler> _logger;

    public EnableCommandInFunctionCommandHandler(ILogger<EnableCommandInFunctionCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(EnableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: EnableCommandInFunctionCommandHandler");

        await PermissionAggregateRoot.EnableCommandInFunction(request.FunctionId, request.CommandId);
        
        _logger.LogInformation("END: EnableCommandInFunctionCommandHandler");
    }
}