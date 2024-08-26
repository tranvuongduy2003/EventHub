using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.EventAggregate;

[Table("FavouriteEvents")]
[PrimaryKey("UserId", "EventId")]
public class FavouriteEvent : EntityBase
{
    [Required]
    public required Guid UserId { get; set; } = Guid.Empty;

    [Required] 
    public required Guid EventId { get; set; } = Guid.Empty;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}