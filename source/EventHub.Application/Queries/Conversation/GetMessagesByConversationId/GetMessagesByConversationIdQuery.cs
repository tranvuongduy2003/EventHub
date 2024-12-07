using EventHub.Application.SeedWork.DTOs.Message;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetMessagesByConversationId;

public record GetMessagesByConversationIdQuery
    (Guid ConversationId, PaginationFilter Filter) : IQuery<Pagination<MessageDto>>;
