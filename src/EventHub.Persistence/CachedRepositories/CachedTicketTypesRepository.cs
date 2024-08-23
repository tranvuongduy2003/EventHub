using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedTicketTypesRepository : CachedRepositoryBase<TicketType>, ICachedTicketTypesRepository
{
    public CachedTicketTypesRepository(ApplicationDbContext context, IRepositoryBase<TicketType> decorated, ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}