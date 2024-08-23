using EventHub.Domain.SeedWork.DomainEvent;
using MediatR;

namespace EventHub.Domain.Events;

public record DisableCommandInFunctionDomainEvent(Guid Id, string FunctionId, string CommandId) : DomainEvent(Id);