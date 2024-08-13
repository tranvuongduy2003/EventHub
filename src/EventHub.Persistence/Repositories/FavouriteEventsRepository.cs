using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class FavouriteEventsRepository : RepositoryBase<FavouriteEvent>, IFavouriteEventsRepository
{
    public FavouriteEventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}