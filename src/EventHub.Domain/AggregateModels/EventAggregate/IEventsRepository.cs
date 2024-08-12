using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.EventAggregate;

public interface IEventsRepository : IRepositoryBase<Event>
{
}