using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface ITicketTypesRepository : IRepositoryBase<TicketType>
{
}