using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class EventSubImagesRepository : RepositoryBase<EventSubImage>, IEventSubImagesRepository
{
    public EventSubImagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}