using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.PermissionAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IPermissionsRepository : IRepositoryBase<Permission>
{
}