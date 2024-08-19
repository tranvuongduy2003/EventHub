using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IMessagesRepository : IRepositoryBase<Message>
{
}