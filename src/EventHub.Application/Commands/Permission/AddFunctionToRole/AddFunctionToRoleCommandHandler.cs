using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Permission.AddFunctionToRole;

public class AddFunctionToRoleCommandHandler : ICommandHandler<AddFunctionToRoleCommand>
{
    private readonly ILogger<AddFunctionToRoleCommandHandler> _logger;

    public AddFunctionToRoleCommandHandler(ILogger<AddFunctionToRoleCommandHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(AddFunctionToRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: AddFunctionToRoleCommandHandler");

        await PermissionAggregateRoot.AddFunctionToRole(request.FunctionId, request.RoleId);
        
        _logger.LogInformation("END: AddFunctionToRoleCommandHandler");
    }
}