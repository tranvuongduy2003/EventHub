using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetConversationsByOrganizerId;

public record GetConversationsByOrganizerIdQuery
    (Guid OrganizerId, PaginationFilter Filter) : IQuery<Pagination<ConversationDto>>;
