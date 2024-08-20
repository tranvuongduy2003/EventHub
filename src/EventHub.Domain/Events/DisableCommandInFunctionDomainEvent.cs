using MediatR;

namespace EventHub.Domain.Events;

public record DisableCommandInFunctionDomainEvent(string FunctionId, string CommandId) : INotification;