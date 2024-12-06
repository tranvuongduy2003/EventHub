using EventHub.Application.DTOs.User;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedFollowers;

/// <summary>
/// Represents a query to retrieve a paginated list of followers for a specific user.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of followers for a user, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user whose followers are to be retrieved.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
/// <summary>
/// Gets the pagination filter to apply to the followers list.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> object representing the pagination parameters, including page size and page number.
/// </param>
public record GetPaginatedFollowersQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<UserDto>>;
