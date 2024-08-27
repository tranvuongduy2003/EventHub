using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedUsers;

public record GetPaginatedUsersQuery(PaginationFilter Filter) : IQuery<Pagination<UserDto>>;