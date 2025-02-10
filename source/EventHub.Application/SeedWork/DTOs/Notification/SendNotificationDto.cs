using System.Text.Json.Serialization;
using EventHub.Domain.Shared.Enums.Notification;

namespace EventHub.Application.SeedWork.DTOs.Notification;

public class SendNotificationDto
{
    public string? Title { get; set; } = string.Empty;

    public string? Message { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ENotificationType Type { get; set; }

    public Guid? InvitationId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? UserFollowerId { get; set; }
}
