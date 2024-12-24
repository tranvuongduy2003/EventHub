using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.Entities;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Permission.RemoveFunctionFromRole;

public class RemoveFunctionFromRoleCommandHandler : ICommandHandler<RemoveFunctionFromRoleCommand>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFunctionFromRoleCommandHandler(IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
    }

    public async Task Handle(RemoveFunctionFromRoleCommand request, CancellationToken cancellationToken)
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

        var permissions = _unitOfWork.Permissions
            .FindByCondition(x =>
                x.FunctionId == request.FunctionId &&
                x.RoleId == request.RoleId)
            .ToList();

        if (!permissions.Any())
        {
            throw new NotFoundException("Permission does not exist!");
        }

        await _unitOfWork.Permissions.DeleteList(permissions);
        await _unitOfWork.CommitAsync();
    }
}
