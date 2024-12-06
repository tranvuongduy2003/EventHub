﻿using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class UserFollowersRepository : RepositoryBase<UserFollower>, IUserFollowersRepository
{
    public UserFollowersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}