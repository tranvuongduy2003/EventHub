using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class RemoveFunctionFromRoleDomainEventHandler : IDomainEventHandler<RemoveFunctionFromRoleDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveFunctionFromRoleDomainEventHandler> _logger;
    private readonly RoleManager<Role> _roleManager;

    public RemoveFunctionFromRoleDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<RemoveFunctionFromRoleDomainEventHandler> logger, RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task Handle(RemoveFunctionFromRoleDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: RemoveFunctionFromRoleDomainEventHandler");

        var isFunctionExisted = await _unitOfWork.Functions.ExistAsync(notification.FunctionId);
        if (!isFunctionExisted)
            throw new NotFoundException("Function does not exist!");

        var role = await _roleManager.FindByIdAsync(notification.RoleId.ToString());
        if (role == null)
            throw new NotFoundException("Role does not exist!");

        var permissions = _unitOfWork.Permissions
            .FindByCondition(x =>
                x.FunctionId.Equals(notification.FunctionId) &&
                x.RoleId.Equals(notification.RoleId));

        if (!permissions.Any())
            throw new NotFoundException("Permission does not exist!");
        
        await _unitOfWork.Permissions.DeleteListAsync(permissions);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: RemoveFunctionFromRoleDomainEventHandler");
    }
}