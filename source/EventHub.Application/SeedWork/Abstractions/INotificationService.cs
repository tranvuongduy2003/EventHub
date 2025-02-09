using EventHub.Application.SeedWork.DTOs.Notification;

namespace EventHub.Application.SeedWork.Abstractions;

public interface INotificationService
{
    Task SendNotificationToAll(SendNotificationDto notification);

    Task SendNotification(string userId, SendNotificationDto notification);
}
