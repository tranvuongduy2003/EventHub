using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate;

[Table("EmailAttachments")]
public class EmailAttachment : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] 
    public required Guid EmailContentId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public required string AttachmentFileName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public required string AttachmentUrl { get; set; } = string.Empty;

    [ForeignKey("EmailContentId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual EmailContent EmailContent { get; set; } = null!;
}
