using MediatR;

namespace EventHub.Domain.Events;

public record EnableCommandInFunctionDomainEvent(string FunctionId, string CommandId) : INotification;