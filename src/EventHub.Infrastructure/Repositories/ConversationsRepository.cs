using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class ConversationsRepository : RepositoryBase<Conversation>, IConversationsRepository
{
    public ConversationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}