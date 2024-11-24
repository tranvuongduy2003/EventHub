﻿using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class ReasonsRepository : RepositoryBase<Reason>, IReasonsRepository
{
    public ReasonsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}