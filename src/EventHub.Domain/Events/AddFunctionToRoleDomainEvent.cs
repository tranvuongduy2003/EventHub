using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that occurs when a function is added to a role.
/// </summary>
/// <param name="Id">The unique identifier for the domain event.</param>
/// <param name="FunctionId">The identifier of the function being added to the role.</param>
/// <param name="RoleId">The unique identifier of the role to which the function is being added.</param>
public record AddFunctionToRoleDomainEvent(Guid Id, string FunctionId, Guid RoleId) : DomainEvent(Id);