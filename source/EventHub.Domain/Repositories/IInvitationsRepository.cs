﻿using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IInvitationsRepository : IRepositoryBase<Invitation>

{
}