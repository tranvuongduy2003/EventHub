using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.ConversationAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IConversationsRepository : IRepositoryBase<Conversation>
{
}