using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedFollowersToInvite;

public record GetPaginatedFollowersToInviteQuery(Guid UserId, Guid EventId, PaginationFilter Filter) : IQuery<Pagination<UserDto>>;
