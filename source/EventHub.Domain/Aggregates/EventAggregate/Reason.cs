using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate;

[Table("Reasons")]
public class Reason : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;

    [Required]
    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public required string Name { get; set; } = string.Empty;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}
