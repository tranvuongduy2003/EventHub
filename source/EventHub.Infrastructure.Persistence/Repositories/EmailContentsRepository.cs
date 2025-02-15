﻿using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class EmailContentsRepository : RepositoryBase<EmailContent>, IEmailContentsRepository
{
    public EmailContentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}