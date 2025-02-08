using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Enums.Notification;

namespace EventHub.Application.Commands.Notification.SeenAll;

public record SeenAllCommand(ENotificationType? Type) : ICommand;
