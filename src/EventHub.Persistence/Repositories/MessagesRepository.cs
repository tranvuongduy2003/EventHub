using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class MessagesRepository : RepositoryBase<Message>, IMessagesRepository
{
    public MessagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}