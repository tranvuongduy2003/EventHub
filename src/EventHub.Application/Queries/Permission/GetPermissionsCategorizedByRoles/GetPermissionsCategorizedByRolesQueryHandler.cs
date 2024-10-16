using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Function;
using EventHub.Shared.DTOs.Permission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EventHub.Application.Queries.Permission.GetPermissionsCategorizedByRoles;

public class
    GetPermissionsCategorizedByRolesQueryHandler : IQueryHandler<GetPermissionsCategorizedByRolesQuery,
    List<RolePermissionDto>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetPermissionsCategorizedByRolesQueryHandler(IUnitOfWork unitOfWork, RoleManager<Role> roleManager,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<List<RolePermissionDto>> Handle(GetPermissionsCategorizedByRolesQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = _unitOfWork.Permissions
            .FindAll(false, x => x.Function);

        var rolePermissions = await _roleManager.Roles
            .LeftJoin(
                permissions,
                r => r.Id,
                p => p.RoleId,
                (role, permission) => new { Role = role, Permission = permission })
            .GroupBy(x => x.Role)
            .Select(group => new RolePermissionDto
            {
                Id = group.Key.Id,
                Name = group.Key.Name ?? "",
                Functions = _mapper.Map<List<FunctionDto>>(
                    group
                        .Select(g => g.Permission.Function)
                        .ToList())
            })
            .ToListAsync();


        return rolePermissions;
    }
}