using EventHub.Domain.SeedWork.DomainEvent;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that occurs when a function is removed from a role.
/// </summary>
/// <param name="Id">The unique identifier for the domain event.</param>
/// <param name="FunctionId">The identifier of the function being removed from the role.</param>
/// <param name="RoleId">The unique identifier of the role from which the function is being removed.</param>
public record RemoveFunctionFromRoleDomainEvent(Guid Id, string FunctionId, Guid RoleId) : DomainEvent(Id);