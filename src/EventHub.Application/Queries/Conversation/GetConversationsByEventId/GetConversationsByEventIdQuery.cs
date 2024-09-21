using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Conversation;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetConversationsByEventId;

public record GetConversationsByEventIdQuery
    (Guid EventId, PaginationFilter Filter) : IQuery<Pagination<ConversationDto>>;