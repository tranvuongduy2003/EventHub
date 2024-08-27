using EventHub.Domain.SeedWork.DomainEvent;
using MediatR;

namespace EventHub.Domain.Events;

/// <summary>
/// Represents a domain event that occurs when a command is disabled within a function.
/// </summary>
/// <param name="Id">The unique identifier for the domain event.</param>
/// <param name="FunctionId">The identifier of the function in which the command is being disabled.</param>
/// <param name="CommandId">The identifier of the command being disabled within the function.</param>
public record DisableCommandInFunctionDomainEvent(Guid Id, string FunctionId, string CommandId) : DomainEvent(Id);