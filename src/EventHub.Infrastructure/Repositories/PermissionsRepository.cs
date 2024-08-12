using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class PermissionsRepository : RepositoryBase<Permission>, IPermissionsRepository
{
    public PermissionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}