using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate.Entities;

[Table("Reviews")]
public class Review : EntityBase, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required Guid AuthorId { get; set; } = Guid.Empty;

    [Required]
    public required Guid EventId { get; set; } = Guid.Empty;

    [MaxLength(1000)]
    [Column(TypeName = "nvarchar(1000)")]
    public string? Content { get; set; }

    public bool IsPositive { get; set; }

    [Required]
    [Range(1.0, 5.0)]
    public required double Rate { get; set; } = 1.0;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;
}
