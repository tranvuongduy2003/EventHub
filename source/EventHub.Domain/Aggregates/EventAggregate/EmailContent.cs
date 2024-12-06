using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EmailLoggerAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate;

[Table("EmailContents")]
public class EmailContent : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(4000)]
    [Column(TypeName = "nvarchar(4000)")]
    public required string Content { get; set; } = string.Empty;

    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<EmailAttachment> EmailAttachments { get; set; } = new List<EmailAttachment>();

    public virtual ICollection<EmailLogger> EmailLoggers { get; set; } = new List<EmailLogger>();
}
