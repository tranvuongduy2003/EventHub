using System.Linq.Expressions;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Infrastructure.Persistence.CachedRepositories;

public class CachedEventsRepository : CachedRepositoryBase<Event>, ICachedEventsRepository
{
    private readonly ICacheService _cacheService;

    public CachedEventsRepository(ApplicationDbContext context, IRepositoryBase<Event> decorated,
        ICacheService cacheService) : base(context, decorated, cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task UpdateAccessStatusAsync(IQueryable<Event> events, bool isPrivate,
        CancellationToken cancellationToken)
    {
        string listKey = $"{typeof(Event).Name}";
        await _cacheService.RemoveData(listKey);

        foreach (Event @event in events)
        {
            string entityKey = $"{typeof(Event).Name}-{@event.Id}";
            await _cacheService.RemoveData(entityKey);
        }

        await events.ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsPrivate, isPrivate),
            cancellationToken);
    }

    public async Task RestoreAsync(IQueryable<Event> events, CancellationToken cancellationToken)
    {
        string listKey = $"{typeof(Event).Name}";
        await _cacheService.RemoveData(listKey);

        foreach (Event @event in events)
        {
            string entityKey = $"{typeof(Event).Name}-{@event.Id}";
            await _cacheService.RemoveData(entityKey);
        }
        
        await events.ExecuteUpdateAsync(setters => setters
            .SetProperty(e => e.IsDeleted, false)
            .SetProperty(e => e.DeletedAt, (DateTime?)null), cancellationToken);
    }
}
