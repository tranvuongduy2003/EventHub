using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.PermissionAggregate;

public interface IPermissionsRepository : IRepositoryBase<Permission>
{
}