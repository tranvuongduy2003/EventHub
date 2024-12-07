using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.CachedRepositories;

public class CachedTicketTypesRepository : CachedRepositoryBase<TicketType>, ICachedTicketTypesRepository
{
    public CachedTicketTypesRepository(ApplicationDbContext context, IRepositoryBase<TicketType> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
    }
}
