using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Function;
using EventHub.Shared.DTOs.Permission;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Permission.GetPermissionsByUser;

public class GetPermissionsByUserQueryHandler : IQueryHandler<GetPermissionsByUserQuery, List<RolePermissionDto>>
{
    private readonly ILogger<GetPermissionsByUserQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetPermissionsByUserQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetPermissionsByUserQueryHandler> logger, RoleManager<Role> roleManager,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<RolePermissionDto>> Handle(GetPermissionsByUserQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPermissionsByUserQueryHandler");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new NotFoundException("User does not exist!");

        var permissions = _unitOfWork.Permissions
            .FindAll(false, x => x.Function);

        var userRoleNames = await _userManager.GetRolesAsync(user);
        var userRoles = _roleManager.Roles
            .AsNoTracking()
            .Join(userRoleNames, r => r.Name, n => n, (role, name) => role);

        var rolePermissions = await userRoles
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


        _logger.LogInformation("END: GetPermissionsByUserQueryHandler");

        return rolePermissions;
    }
}