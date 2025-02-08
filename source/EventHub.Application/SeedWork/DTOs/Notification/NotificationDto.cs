using System.Text.Json.Serialization;
using EventHub.Application.SeedWork.DTOs.Invitation;
using EventHub.Application.SeedWork.DTOs.Payment;
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

    public bool IsSeen { get; set; }

    public Guid? TargetUserId { get; set; }

    public string? TargetGroup { get; set; }

    public Guid? InvitationId { get; set; } = null!;

    public Guid? PaymentId { get; set; } = null!;

    public Guid? FollowerId { get; set; } = null!;

    public InvitationDto? Invitation { get; set; } = null!;

    public PaymentDto? Payment { get; set; } = null!;

    public UserDto? Follower { get; set; } = null!;
}
