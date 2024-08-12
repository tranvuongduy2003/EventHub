using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
{
    public TicketsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}