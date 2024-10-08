﻿using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.ConversationAggregate;
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