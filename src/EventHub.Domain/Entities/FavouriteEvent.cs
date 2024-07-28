using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("FavouriteEvents")]
[PrimaryKey("UserId", "EventId")]
public class FavouriteEvent : EntityBase
{
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string UserId { get; set; } = string.Empty;

    [Required] public int EventId { get; set; }

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual User User { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}