using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.ConversationAggregate;

public interface IMessagesRepository : IRepositoryBase<Message>
{
}