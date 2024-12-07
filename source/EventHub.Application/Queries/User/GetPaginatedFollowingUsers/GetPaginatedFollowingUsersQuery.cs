using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedFollowingUsers;

/// <summary>
/// Represents a query to retrieve a paginated list of users that a specific user is following.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of users that a specified user is following, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user whose followed users are to be retrieved.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
/// <summary>
/// Gets the pagination filter to apply to the list of followed users.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> object representing the pagination parameters, including page size and page number.
/// </param>
public record GetPaginatedFollowingUsersQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<UserDto>>;
