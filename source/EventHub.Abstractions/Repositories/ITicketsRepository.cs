using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.TicketAggregate;

namespace EventHub.Abstractions.Repositories;

public interface ITicketsRepository : IRepositoryBase<Ticket>
{
}