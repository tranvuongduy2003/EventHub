﻿using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
{
    public TicketsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}