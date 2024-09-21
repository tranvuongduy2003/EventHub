using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class TicketTypesRepository : RepositoryBase<TicketType>, ITicketTypesRepository
{
    public TicketTypesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}