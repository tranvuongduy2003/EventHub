using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class RemoveFunctionFromRoleDomainEventHandler : IDomainEventHandler<RemoveFunctionFromRoleDomainEvent>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFunctionFromRoleDomainEventHandler(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task Handle(RemoveFunctionFromRoleDomainEvent notification, CancellationToken cancellationToken)
    {
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
    }
}