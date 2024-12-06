using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Function;
using EventHub.Application.DTOs.Permission;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        List<Domain.Aggregates.PermissionAggregate.Permission> permissions = await _unitOfWork.Permissions
            .FindAll(false, x => x.Function)
            .ToListAsync(cancellationToken);

        var roles = _roleManager.Roles.ToList();

        var rolePermissions = (
                from _role in roles
                join _permission in permissions.DefaultIfEmpty()
                    on _role.Id equals _permission.RoleId
                select new { Role = _role, Permission = _permission }
            )
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
            .ToList();


        return rolePermissions;
    }
}
