using EventHub.Abstractions;
using EventHub.Abstractions.CachedRepositories;
using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedCategoriesRepository : CachedRepositoryBase<Category>, ICachedCategoriesRepository
{
    public CachedCategoriesRepository(ApplicationDbContext context, IRepositoryBase<Category> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}