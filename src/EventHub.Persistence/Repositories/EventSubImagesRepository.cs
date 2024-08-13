using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class EventSubImagesRepository : RepositoryBase<EventSubImage>, IEventSubImagesRepository
{
    public EventSubImagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}