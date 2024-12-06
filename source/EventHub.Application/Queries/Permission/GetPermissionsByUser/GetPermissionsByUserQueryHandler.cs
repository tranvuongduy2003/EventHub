using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Function;
using EventHub.Application.DTOs.Permission;
using EventHub.Application.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Permission.GetPermissionsByUser;

public class GetPermissionsByUserQueryHandler : IQueryHandler<GetPermissionsByUserQuery, List<RolePermissionDto>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPermissionsByUserQueryHandler(IUnitOfWork unitOfWork, RoleManager<Role> roleManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<RolePermissionDto>> Handle(GetPermissionsByUserQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User does not exist!");
        }

        var permissions = _unitOfWork.Permissions
            .FindAll(false, x => x.Function)
            .ToList();

        IList<string> userRoleNames = await _userManager.GetRolesAsync(user);
        var userRoles = _roleManager.Roles
            .AsNoTracking()
            .Join(userRoleNames, r => r.Name, n => n, (role, name) => role)
            .ToList();

        var rolePermissions = (
                from _role in userRoles
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
