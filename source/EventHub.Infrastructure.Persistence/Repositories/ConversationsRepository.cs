using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class ConversationsRepository : RepositoryBase<Conversation>, IConversationsRepository
{
    public ConversationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}