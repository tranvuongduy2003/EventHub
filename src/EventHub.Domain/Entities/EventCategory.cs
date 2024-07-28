using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("EventCategories")]
[PrimaryKey("CategoryId", "EventId")]
public class EventCategory : EntityBase
{
    [Required] public int CategoryId { get; set; }

    [Required] public int EventId { get; set; }

    [ForeignKey("CategoryId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}