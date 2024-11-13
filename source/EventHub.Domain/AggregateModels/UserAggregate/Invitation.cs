using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.UserAggregate;

[Table("Invitations")]
[PrimaryKey("InviterId", "InvitedId", "EventId")]
public class Invitation : EntityBase
{
    [Required]
    public Guid InviterId { get; set; } = Guid.Empty;

    [Required]
    public Guid InvitedId { get; set; } = Guid.Empty;

    [Required] 
    public Guid EventId { get; set; } = Guid.Empty;

    [ForeignKey("InviterId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Inviter { get; set; } = null!;

    [ForeignKey("InvitedId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Invited { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;
}