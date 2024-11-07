using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class PermissionsRepository : RepositoryBase<Permission>, IPermissionsRepository
{
    public PermissionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}