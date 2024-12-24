using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Permission.AddFunctionToRole;

public class AddFunctionToRoleCommandHandler : ICommandHandler<AddFunctionToRoleCommand>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public AddFunctionToRoleCommandHandler(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task Handle(AddFunctionToRoleCommand request, CancellationToken cancellationToken)
    {
        bool isFunctionExisted = await _unitOfWork.Functions.ExistAsync(request.FunctionId);
        if (!isFunctionExisted)
        {
            throw new NotFoundException("Function does not exist!");
        }

        Role role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null)
        {
            throw new NotFoundException("Role does not exist!");
        }

        bool isPermissionExisted = await _unitOfWork.Permissions
            .ExistAsync(x =>
                x.FunctionId == request.FunctionId &&
                x.RoleId == request.RoleId);

        if (isPermissionExisted)
        {
            throw new BadRequestException("Permission already existed!");
        }

        List<Domain.Aggregates.UserAggregate.ValueObjects.Permission> permissions = await _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId == request.FunctionId)
            .Select(x => new Domain.Aggregates.UserAggregate.ValueObjects.Permission
            {
                FunctionId = x.FunctionId,
                RoleId = request.RoleId,
                CommandId = x.CommandId
            })
            .ToListAsync(cancellationToken);

        await _unitOfWork.Permissions.CreateListAsync(permissions);
        await _unitOfWork.CommitAsync();
    }
}
