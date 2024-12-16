using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Permission;
using EventHub.Domain.Aggregates.UserAggregate;
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

    public GetPermissionsCategorizedByRolesQueryHandler(RoleManager<Role> roleManager,
        IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public Task<List<RolePermissionDto>> Handle(GetPermissionsCategorizedByRolesQuery request,
        CancellationToken cancellationToken)
    {
        var userRoles = _roleManager.Roles
            .AsNoTracking()
            .Include(x => x.Permissions)
            .ThenInclude(x => x.Function)
            .ToList();

        List<RolePermissionDto> rolePermissions = _mapper.Map<List<RolePermissionDto>>(userRoles);

        return Task.FromResult(rolePermissions);
    }
}
