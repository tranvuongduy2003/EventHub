using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class EventCategoriesRepository : RepositoryBase<EventCategory>, IEventCategoriesRepository
{
    public EventCategoriesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}