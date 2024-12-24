using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.EventAggregate.ValueObjects;

[Table("EventCategories")]
[PrimaryKey("CategoryId", "EventId")]
public class EventCategory : EntityBase
{
    [Required] 
    public required Guid CategoryId { get; set; } = Guid.Empty;

    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;

    [ForeignKey("CategoryId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}
