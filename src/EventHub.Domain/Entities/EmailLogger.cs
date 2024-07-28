using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("EmailLoggers")]
public class EmailLogger : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string ReceiverEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string SentEmail { get; set; } = string.Empty;

    [Required] public int EmailContentId { get; set; }

    [ForeignKey("EmailContentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual EmailContent EmailContent { get; set; } = null!;
}