using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;

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
        bool isFunctionExisted = await _unitOfWork.Functions.ExistAsync(notification.FunctionId);
        if (!isFunctionExisted)
        {
            throw new NotFoundException("Function does not exist!");
        }

        Role role = await _roleManager.FindByIdAsync(notification.RoleId.ToString());
        if (role == null)
        {
            throw new NotFoundException("Role does not exist!");
        }

        var permissions = _unitOfWork.Permissions
            .FindByCondition(x =>
                x.FunctionId == notification.FunctionId &&
                x.RoleId == notification.RoleId)
            .ToList();

        if (!permissions.Any())
        {
            throw new NotFoundException("Permission does not exist!");
        }

        await _unitOfWork.Permissions.DeleteList(permissions);
        await _unitOfWork.CommitAsync();
    }
}
