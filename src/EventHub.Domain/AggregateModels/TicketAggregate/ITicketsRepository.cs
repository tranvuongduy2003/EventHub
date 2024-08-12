using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.TicketAggregate;

public interface ITicketsRepository : IRepositoryBase<Ticket>
{
}