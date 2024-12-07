using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetConversationsByEventId;

public record GetConversationsByEventIdQuery
    (Guid EventId, PaginationFilter Filter) : IQuery<Pagination<ConversationDto>>;
