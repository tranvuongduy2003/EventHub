using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class PermissionsRepository : RepositoryBase<Permission>, IPermissionsRepository
{
    public PermissionsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}