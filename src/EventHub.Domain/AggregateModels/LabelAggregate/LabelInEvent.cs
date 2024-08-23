using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.LabelAggregate;

[Table("LabelInEvents")]
[PrimaryKey("LabelId", "EventId")]
public class LabelInEvent : EntityBase
{
    [Required] public Guid LabelId { get; set; }

    [Required] public Guid EventId { get; set; }

    [ForeignKey("LabelId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Label Label { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Event Event { get; set; } = null!;
}