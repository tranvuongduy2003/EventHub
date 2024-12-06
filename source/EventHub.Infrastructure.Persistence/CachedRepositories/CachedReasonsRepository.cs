using EventHub.Application.Abstractions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.CachedRepositories;

public class CachedReasonsRepository : CachedRepositoryBase<Reason>, ICachedReasonsRepository
{
    public CachedReasonsRepository(ApplicationDbContext context, IRepositoryBase<Reason> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}
