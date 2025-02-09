using EventHub.Application.SeedWork.DTOs.Notification;

namespace EventHub.Application.SeedWork.Abstractions;

public interface INotificationService
{
    Task SendNotificationToAll(NotificationDto notification);

    Task SendNotificationToGroup(string groupName, SendNotificationDto notification);

    Task SendNotificationToUser(string userId, NotificationDto notification);
}
