using EventHub.Application.SeedWork.DTOs.Permission;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Permission.GetPermissionsByUser;

/// <summary>
/// Represents a query to retrieve a list of permissions associated with a specific user.
/// </summary>
/// <remarks>
/// This query is used to fetch the permissions assigned to a user based on their roles.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user whose permissions are to be retrieved.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
public record GetPermissionsByUserQuery(Guid UserId) : IQuery<List<RolePermissionDto>>;