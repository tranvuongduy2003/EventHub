﻿using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface ILabelsRepository : IRepositoryBase<Label>
{
}