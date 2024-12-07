using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.CategoryAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.CachedRepositories;

public class CachedCategoriesRepository : CachedRepositoryBase<Category>, ICachedCategoriesRepository
{
    public CachedCategoriesRepository(ApplicationDbContext context, IRepositoryBase<Category> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}
