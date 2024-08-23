using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.AggregateRoot;

namespace EventHub.Domain.AggregateModels.PermissionAggregate;

public class PermissionAggregateRoot : AggregateRoot
{
    public static async Task EnableCommandInFunction(string functionId, string commandId)
    {
        new PermissionAggregateRoot().RaiseDomainEvent(new EnableCommandInFunctionDomainEvent(Guid.NewGuid(), functionId, commandId));
    }
    
    public static async Task DisableCommandInFunction(string functionId, string commandId)
    {
        new PermissionAggregateRoot().RaiseDomainEvent(new DisableCommandInFunctionDomainEvent(Guid.NewGuid(), functionId, commandId));
    }
}