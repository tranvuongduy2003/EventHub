using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.Shared.Enums.Notification;

namespace EventHub.Application.SeedWork.DTOs.Notification;

public class NotificationDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; } = string.Empty;

    public string? Message { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ENotificationType Type { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public Guid? TargetUserId { get; set; }

    public string? TargetGroup { get; set; }

    public UserDto TargetUser { get; set; } = null!;
}
