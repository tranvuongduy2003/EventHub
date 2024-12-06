using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.LabelAggregate;

[Table("LabelInUsers")]
[PrimaryKey("LabelId", "AuthorId")]
public class LabelInUser : EntityBase
{
    [Required] public required Guid LabelId { get; set; } = Guid.Empty;

    [Required] public required Guid UserId { get; set; } = Guid.Empty;

    [ForeignKey("LabelId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Label Label { get; set; } = null!;

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User User { get; set; } = null!;
}