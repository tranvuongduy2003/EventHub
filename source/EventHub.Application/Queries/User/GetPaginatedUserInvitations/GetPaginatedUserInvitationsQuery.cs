using EventHub.Application.SeedWork.DTOs.Invitation;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.User.GetPaginatedUserInvitations;

public record GetPaginatedUserInvitationsQuery(PaginationFilter Filter) : IQuery<Pagination<InvitationDto>>;
