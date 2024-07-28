using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class EventCategoriesRepository : RepositoryBase<EventCategory>, IEventCategoriesRepository
{
    public EventCategoriesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}