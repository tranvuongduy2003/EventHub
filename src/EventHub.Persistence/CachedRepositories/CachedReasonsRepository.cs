using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedReasonsRepository : CachedRepositoryBase<Reason>, ICachedReasonsRepository
{
    public CachedReasonsRepository(ApplicationDbContext context, IRepositoryBase<Reason> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}