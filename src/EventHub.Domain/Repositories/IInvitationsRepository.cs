﻿using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IInvitationsRepository : IRepositoryBase<Invitation>

{
}