using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class TicketTypesRepository : RepositoryBase<TicketType>, ITicketTypesRepository
{
    public TicketTypesRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}