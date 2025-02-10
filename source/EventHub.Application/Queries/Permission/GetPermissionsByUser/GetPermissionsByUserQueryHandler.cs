using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Permission;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Permission.GetPermissionsByUser;

public class GetPermissionsByUserQueryHandler : IQueryHandler<GetPermissionsByUserQuery, List<RolePermissionDto>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Domain.Aggregates.UserAggregate.Entities.Role> _roleManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPermissionsByUserQueryHandler(RoleManager<Domain.Aggregates.UserAggregate.Entities.Role> roleManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
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

        IList<string> userRoleNames = await _userManager.GetRolesAsync(user);
        var userRoles = _roleManager.Roles
            .AsNoTracking()
            .Join(userRoleNames, _role => _role.Name, _name => _name, (_role, _name) => _role)
            .Include(x => x.Permissions)
            .ThenInclude(x => x.Function)
            .ToList();

        List<RolePermissionDto> rolePermissions = _mapper.Map<List<RolePermissionDto>>(userRoles);

        return rolePermissions;
    }
}
