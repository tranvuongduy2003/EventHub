using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.Shared.Enums.Notification;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.NotificationAggregate;

[Table("Notifications")]
public class Notification : AggregateRoot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Title { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")]
    public string? Message { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ENotificationType Type { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public bool IsSeen { get; set; }

    public Guid? TargetUserId { get; set; }

    public string? TargetGroup { get; set; }

    public Guid? InvitationId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? UserFollowerId { get; set; }

    [ForeignKey("TargetUserId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User TargetUser { get; set; } = null!;

    [ForeignKey("InvitationId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Invitation? Invitation { get; set; } = null!;

    [ForeignKey("PaymentId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Payment? Payment { get; set; } = null!;

    [ForeignKey("UserFollowerId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual UserFollower? UserFollower { get; set; } = null!;
}
