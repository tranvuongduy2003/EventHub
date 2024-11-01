using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        var isFunctionExisted = await _unitOfWork.Functions.ExistAsync(notification.FunctionId);
        if (!isFunctionExisted)
            throw new NotFoundException("Function does not exist!");

        var role = await _roleManager.FindByIdAsync(notification.RoleId.ToString());
        if (role == null)
            throw new NotFoundException("Role does not exist!");

        var isPermissionExisted = await _unitOfWork.Permissions
            .ExistAsync(x =>
                x.FunctionId.Equals(notification.FunctionId) &&
                x.RoleId.Equals(notification.RoleId));

        if (isPermissionExisted)
            throw new BadRequestException("Permission already existed!");

        var permissions = await _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId.Equals(notification.FunctionId))
            .Select(x => new Permission
            {
                FunctionId = x.FunctionId,
                RoleId = notification.RoleId,
                CommandId = x.CommandId
            })
            .ToListAsync();

        await _unitOfWork.Permissions.CreateListAsync(permissions);
        await _unitOfWork.CommitAsync();
    }
}