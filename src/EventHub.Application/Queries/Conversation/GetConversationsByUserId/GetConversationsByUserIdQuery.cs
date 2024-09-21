using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Conversation;
using EventHub.Shared.SeedWork;

namespace EventHub.Application.Queries.Conversation.GetConversationsByUserId;

public record GetConversationsByUserIdQuery(Guid UserId, PaginationFilter Filter) : IQuery<Pagination<ConversationDto>>;