using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class EventsRepository : RepositoryBase<Event>, IEventsRepository
{
    public EventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}