using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class EventsRepository : RepositoryBase<Event>, IEventsRepository
{
    public EventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}