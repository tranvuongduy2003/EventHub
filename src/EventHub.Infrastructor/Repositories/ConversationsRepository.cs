using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class ConversationsRepository : RepositoryBase<Conversation>, IConversationsRepository
{
    public ConversationsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}