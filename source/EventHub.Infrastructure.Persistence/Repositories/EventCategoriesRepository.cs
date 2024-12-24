using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class EventCategoriesRepository : RepositoryBase<EventCategory>, IEventCategoriesRepository
{
    public EventCategoriesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}