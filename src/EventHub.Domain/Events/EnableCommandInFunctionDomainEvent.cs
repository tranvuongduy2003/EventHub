using EventHub.Domain.SeedWork.DomainEvent;
using MediatR;

namespace EventHub.Domain.Events;

public record EnableCommandInFunctionDomainEvent(Guid Id, string FunctionId, string CommandId) : DomainEvent(Id);