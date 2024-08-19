using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedCategoriesRepository : CachedRepositoryBase<Category>, ICachedCategoriesRepository
{
    public CachedCategoriesRepository(ApplicationDbContext context, RepositoryBase<Category> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}