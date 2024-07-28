using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class EventSubImagesRepository : RepositoryBase<EventSubImage>, IEventSubImagesRepository
{
    public EventSubImagesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}