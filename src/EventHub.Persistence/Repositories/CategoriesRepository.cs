﻿using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
{
    public CategoriesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}