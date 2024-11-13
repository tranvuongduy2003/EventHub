using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.SeedWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.ReviewAggregate;

[Table("Reviews")]
public class Review : AggregateRoot, IAuditable
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