using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Permission;

namespace EventHub.Application.Queries.Permission.GetPermissionsByUser;

public record GetPermissionsByUserQuery(Guid UserId) : IQuery<List<RolePermissionDto>>;