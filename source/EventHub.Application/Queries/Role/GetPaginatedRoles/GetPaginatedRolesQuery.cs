using EventHub.Application.SeedWork.DTOs.Role;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Role.GetPaginatedRoles;

public record GetPaginatedRolesQuery(PaginationFilter Filter) : IQuery<Pagination<RoleDto>>;
