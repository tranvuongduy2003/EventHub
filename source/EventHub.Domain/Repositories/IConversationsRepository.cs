﻿using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IConversationsRepository : IRepositoryBase<Conversation>
{
}
