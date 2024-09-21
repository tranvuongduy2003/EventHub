using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Permission;

namespace EventHub.Application.Queries.Permission.GetFullPermissions;

/// <summary>
/// Represents a query to retrieve a list of full permissions.
/// </summary>
/// <remarks>
/// This record implements the <see cref="IQuery{List}"/> interface, which signifies
/// that it is used to request a list of <see cref="FullPermissionDto"/> objects. The query is designed
/// to encapsulate the request for fetching all available permissions, typically used in scenarios where
/// a comprehensive list of permissions is needed, such as in user role management or access control systems.
/// </remarks>
public record GetFullPermissionsQuery : IQuery<List<FullPermissionDto>>;