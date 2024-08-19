using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class FunctionsRepository : RepositoryBase<Function>, IFunctionsRepository
{
    public FunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}