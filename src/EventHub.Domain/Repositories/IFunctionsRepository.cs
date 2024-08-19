﻿using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IFunctionsRepository : IRepositoryBase<Function>
{
}