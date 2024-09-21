using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class EventCategoriesRepository : RepositoryBase<EventCategory>, IEventCategoriesRepository
{
    public EventCategoriesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}