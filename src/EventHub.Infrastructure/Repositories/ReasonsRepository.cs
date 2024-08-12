using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class ReasonsRepository : RepositoryBase<Reason>, IReasonsRepository
{
    public ReasonsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}