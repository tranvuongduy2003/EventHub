using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Domain.Repositories;

public interface IFavouriteEventsRepository : IRepositoryBase<FavouriteEvent>
{
}
