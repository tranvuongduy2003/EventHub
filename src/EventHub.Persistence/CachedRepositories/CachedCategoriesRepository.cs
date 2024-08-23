using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedCategoriesRepository : CachedRepositoryBase<Category>, ICachedCategoriesRepository
{
    public CachedCategoriesRepository(ApplicationDbContext context, IRepositoryBase<Category> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}