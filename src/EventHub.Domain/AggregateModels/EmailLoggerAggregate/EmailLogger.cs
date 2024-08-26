using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.EmailLoggerAggregate;

[Table("EmailLoggers")]
public class EmailLogger : AggregateRoot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string ReceiverEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string SentEmail { get; set; } = string.Empty;

    [Required] 
    public required Guid EmailContentId { get; set; } = Guid.Empty;

    [ForeignKey("EmailContentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual EmailContent EmailContent { get; set; } = null!;
}