using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.ConversationAggregate.Entities;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class MessagesRepository : RepositoryBase<Message>, IMessagesRepository
{
    public MessagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}