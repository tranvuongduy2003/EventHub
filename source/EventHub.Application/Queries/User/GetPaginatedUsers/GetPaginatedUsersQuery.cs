using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedUsers;

/// <summary>
/// Represents a query to retrieve a paginated list of users.
/// </summary>
/// <remarks>
/// This query is used to fetch a list of users, with support for pagination.
/// </remarks>
/// <summary>
/// Gets the pagination filter to apply to the users list.
/// </summary>
/// <param name="Filter">
/// A <see cref="PaginationFilter"/> object representing the pagination parameters, including page size and page number.
/// </param>
public record GetPaginatedUsersQuery(PaginationFilter Filter) : IQuery<Pagination<UserDto>>;
