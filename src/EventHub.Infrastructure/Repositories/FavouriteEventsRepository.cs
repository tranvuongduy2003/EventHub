using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class FavouriteEventsRepository : RepositoryBase<FavouriteEvent>, IFavouriteEventsRepository
{
    public FavouriteEventsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}