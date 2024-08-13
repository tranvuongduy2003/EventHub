using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Services;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedTicketTypesRepository : CachedRepositoryBase<TicketType>
{
    public CachedTicketTypesRepository(ApplicationDbContext context, RepositoryBase<TicketType> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}