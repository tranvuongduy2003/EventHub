using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("EmailContents")]
public class EmailContent : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Content { get; set; } = string.Empty;

    [Required] public int EventId { get; set; }

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<EmailAttachment> EmailAttachments { get; set; } = new List<EmailAttachment>();

    public virtual ICollection<EmailLogger> EmailLoggers { get; set; } = new List<EmailLogger>();
}