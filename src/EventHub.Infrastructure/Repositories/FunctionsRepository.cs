using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class FunctionsRepository : RepositoryBase<Function>, IFunctionsRepository
{
    public FunctionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}