using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedFollowingUsers;

public record GetPaginatedFollowingUsersQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<UserDto>>;