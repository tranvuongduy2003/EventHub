﻿using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IEventCategoriesRepository : IRepositoryBase<EventCategory>
{
}
