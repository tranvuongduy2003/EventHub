using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.RemoveFunctionFromRole;

public class RemoveFunctionFromRoleCommandHandler : ICommandHandler<RemoveFunctionFromRoleCommand>
{
    private readonly ILogger<RemoveFunctionFromRoleCommandHandler> _logger;

    public RemoveFunctionFromRoleCommandHandler(ILogger<RemoveFunctionFromRoleCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(RemoveFunctionFromRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: RemoveFunctionFromRoleCommandHandler");

        await PermissionAggregateRoot.RemoveFunctionFromRole(request.FunctionId, request.RoleId);
        
        _logger.LogInformation("END: RemoveFunctionFromRoleCommandHandler");
    }
}