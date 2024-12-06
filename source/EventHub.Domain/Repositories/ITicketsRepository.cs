using EventHub.Domain.Aggregates.TicketAggregate;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ITicketsRepository : IRepositoryBase<Ticket>
{
}