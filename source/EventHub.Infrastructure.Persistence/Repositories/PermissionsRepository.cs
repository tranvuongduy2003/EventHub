using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Repositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;

namespace EventHub.Infrastructure.Persistence.Repositories;

public class PermissionsRepository : RepositoryBase<Permission>, IPermissionsRepository
{
    public PermissionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}