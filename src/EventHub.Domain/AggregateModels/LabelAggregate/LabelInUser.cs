using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.LabelAggregate;

[Table("LabelInUsers")]
[PrimaryKey("LabelId", "UserId")]
public class LabelInUser : EntityBase
{
    [Required] 
    public required Guid LabelId { get; set; } = Guid.Empty;

    [Required]
    public required Guid UserId { get; set; } = Guid.Empty;

    [ForeignKey("LabelId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Label Label { get; set; } = null!;

    [ForeignKey("UserId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;
}