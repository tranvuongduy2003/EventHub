using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EmailLoggerAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.EventAggregate;

[Table("EmailContents")]
public class EmailContent : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Content { get; set; } = string.Empty;

    [Required] public Guid EventId { get; set; }

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<EmailAttachment> EmailAttachments { get; set; } = new List<EmailAttachment>();

    public virtual ICollection<EmailLogger> EmailLoggers { get; set; } = new List<EmailLogger>();
}