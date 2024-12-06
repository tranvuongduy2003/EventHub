using EventHub.Application.DTOs.Permission;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Permission.GetPermissionsCategorizedByRoles;

/// <summary>
/// Represents a query to retrieve permissions categorized by roles.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IQuery{List}"/> interface, indicating that it is used
/// to request a list of <see cref="RolePermissionDto"/> objects. The query is designed to fetch permissions grouped
/// by roles, providing a structured view of which permissions are associated with each role. This is typically useful
/// in scenarios involving role-based access control or permissions management systems.
/// </remarks>
public class GetPermissionsCategorizedByRolesQuery : IQuery<List<RolePermissionDto>>;