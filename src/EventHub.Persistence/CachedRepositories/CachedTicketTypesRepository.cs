using EventHub.Abstractions;
using EventHub.Abstractions.CachedRepositories;
using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Abstractions.Services;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.CachedRepositories;

public class CachedTicketTypesRepository : CachedRepositoryBase<TicketType>, ICachedTicketTypesRepository
{
    public CachedTicketTypesRepository(ApplicationDbContext context, IRepositoryBase<TicketType> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}