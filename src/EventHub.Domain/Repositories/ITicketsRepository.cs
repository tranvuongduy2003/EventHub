using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface ITicketsRepository : IRepositoryBase<Ticket>
{
}