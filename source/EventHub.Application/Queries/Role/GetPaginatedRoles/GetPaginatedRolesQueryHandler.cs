using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Role;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Role.GetPaginatedRoles;

public class GetPaginatedRolesQueryHandler : IQueryHandler<GetPaginatedRolesQuery, Pagination<RoleDto>>
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Domain.Aggregates.UserAggregate.Entities.Role> _roleManager;

    public GetPaginatedRolesQueryHandler(RoleManager<Domain.Aggregates.UserAggregate.Entities.Role> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<Pagination<RoleDto>> Handle(GetPaginatedRolesQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.Aggregates.UserAggregate.Entities.Role> roles = await _roleManager.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        Pagination<Domain.Aggregates.UserAggregate.Entities.Role> paginatedRoles =
            PagingHelper.QueryPaginate(request.Filter, roles.AsQueryable());

        Pagination<RoleDto> paginatedRoleDtos = _mapper.Map<Pagination<RoleDto>>(paginatedRoles);

        return paginatedRoleDtos;
    }
}
