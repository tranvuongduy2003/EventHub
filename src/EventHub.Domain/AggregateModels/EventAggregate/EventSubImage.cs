using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.EventAggregate;

[Table("EventSubImages")]
public class EventSubImage : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] public Guid EventId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Image { get; set; } = string.Empty;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}