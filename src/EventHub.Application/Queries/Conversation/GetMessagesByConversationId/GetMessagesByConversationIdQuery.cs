using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Message;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetMessagesByConversationId;

public record GetMessagesByConversationIdQuery
    (Guid ConversationId, PaginationFilter Filter) : IQuery<Pagination<MessageDto>>;