using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class MessagesRepository : RepositoryBase<Message>, IMessagesRepository
{
    public MessagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}