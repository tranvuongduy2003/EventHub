using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class AddFunctionToRoleDomainEventHandler : IDomainEventHandler<AddFunctionToRoleDomainEvent>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public AddFunctionToRoleDomainEventHandler(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task Handle(AddFunctionToRoleDomainEvent notification, CancellationToken cancellationToken)
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

        bool isPermissionExisted = await _unitOfWork.Permissions
            .ExistAsync(x =>
                x.FunctionId.Equals(notification.FunctionId, StringComparison.Ordinal) &&
                x.RoleId.Equals(notification.RoleId));

        if (isPermissionExisted)
        {
            throw new BadRequestException("Permission already existed!");
        }

        List<Permission> permissions = await _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId.Equals(notification.FunctionId, StringComparison.Ordinal))
            .Select(x => new Permission
            {
                FunctionId = x.FunctionId,
                RoleId = notification.RoleId,
                CommandId = x.CommandId
            })
            .ToListAsync(cancellationToken);

        await _unitOfWork.Permissions.CreateListAsync(permissions);
        await _unitOfWork.CommitAsync();
    }
}
