using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Notification.DeleteNotification;

public record DeleteNotificationCommand(Guid Id) : ICommand;
