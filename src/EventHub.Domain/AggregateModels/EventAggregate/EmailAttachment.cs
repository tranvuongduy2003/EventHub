using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.EventAggregate;

[Table("EmailAttachments")]
public class EmailAttachment : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int EmailContentId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Attachment { get; set; } = string.Empty;

    [ForeignKey("EmailContentId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual EmailContent EmailContent { get; set; } = null!;
}