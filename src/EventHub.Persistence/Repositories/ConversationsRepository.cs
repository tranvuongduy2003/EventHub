using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class ConversationsRepository : RepositoryBase<Conversation>, IConversationsRepository
{
    public ConversationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}