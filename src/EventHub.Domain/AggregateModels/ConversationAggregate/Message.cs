using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.ConversationAggregate;

[Table("Messages")]
public class Message : EntityAuditBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Content { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Image { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Video { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Audio { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public Guid UserId { get; set; } = Guid.Empty;

    [Required] public Guid EventId { get; set; }

    [Required] public Guid ConversationId { get; set; }

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("ConversationId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Conversation Conversation { get; set; } = null!;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;
}