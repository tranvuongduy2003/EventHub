using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedReasonsRepository : CachedRepositoryBase<Reason>
{
    public CachedReasonsRepository(ApplicationDbContext context, RepositoryBase<Reason> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}