using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class MessagesRepository : RepositoryBase<Message>, IMessagesRepository
{
    public MessagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}