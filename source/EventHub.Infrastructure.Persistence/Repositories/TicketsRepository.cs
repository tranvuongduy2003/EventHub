using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class TicketsRepository : RepositoryBase<Ticket>, ITicketsRepository
{
    public TicketsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}