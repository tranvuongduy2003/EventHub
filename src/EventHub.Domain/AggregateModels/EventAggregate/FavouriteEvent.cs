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
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public Guid UserId { get; set; } = Guid.Empty;

    [Required] public Guid EventId { get; set; }

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}