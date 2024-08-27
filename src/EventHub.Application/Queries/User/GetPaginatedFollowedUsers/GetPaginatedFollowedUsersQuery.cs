using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedFollowedUsers;

public record GetPaginatedFollowedUsersQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<UserDto>>;