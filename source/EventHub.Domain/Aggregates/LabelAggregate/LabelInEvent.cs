using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.LabelAggregate;

[Table("LabelInEvents")]
[PrimaryKey("LabelId", "EventId")]
public class LabelInEvent : EntityBase
{
    [Required] 
    public required Guid LabelId { get; set; } = Guid.Empty;

    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;

    [ForeignKey("LabelId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Label Label { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;
}