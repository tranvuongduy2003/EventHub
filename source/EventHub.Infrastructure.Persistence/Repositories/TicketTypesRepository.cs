﻿using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class TicketTypesRepository : RepositoryBase<TicketType>, ITicketTypesRepository
{
    public TicketTypesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}