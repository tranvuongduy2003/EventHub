using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.EventAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IEventCategoriesRepository : IRepositoryBase<EventCategory>
{
}