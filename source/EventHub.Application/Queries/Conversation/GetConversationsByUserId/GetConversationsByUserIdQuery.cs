using EventHub.Application.DTOs.Conversation;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetConversationsByUserId;

public record GetConversationsByUserIdQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<ConversationDto>>;
