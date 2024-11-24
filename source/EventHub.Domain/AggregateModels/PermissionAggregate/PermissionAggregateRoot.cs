using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;

namespace EventHub.Domain.AggregateModels.PermissionAggregate;

public class PermissionAggregateRoot : AggregateRoot
{
    public static void EnableCommandInFunction(string functionId, string commandId)
    {
        new PermissionAggregateRoot().RaiseDomainEvent(new EnableCommandInFunctionDomainEvent(Guid.NewGuid(), functionId, commandId));
    }

    public static void DisableCommandInFunction(string functionId, string commandId)
    {
        new PermissionAggregateRoot().RaiseDomainEvent(new DisableCommandInFunctionDomainEvent(Guid.NewGuid(), functionId, commandId));
    }

    public static void AddFunctionToRole(string functionId, Guid roleId)
    {
        new PermissionAggregateRoot().RaiseDomainEvent(new AddFunctionToRoleDomainEvent(Guid.NewGuid(), functionId, roleId));
    }

    public static void RemoveFunctionFromRole(string functionId, Guid roleId)
    {
        new PermissionAggregateRoot().RaiseDomainEvent(new RemoveFunctionFromRoleDomainEvent(Guid.NewGuid(), functionId, roleId));
    }
}
