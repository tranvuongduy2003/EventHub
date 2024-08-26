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

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? Content { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? ImageUrl { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? ImageFileName { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? VideoUrl { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? VideoFileName { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? AudioUrl { get; set; }

    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string? AudioFileName { get; set; }

    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;

    [Required] 
    public required Guid ConversationId { get; set; } = Guid.Empty;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("ConversationId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Conversation Conversation { get; set; } = null!;
}