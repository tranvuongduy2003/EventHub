﻿using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IEmailContentsRepository : IRepositoryBase<EmailContent>
{
}