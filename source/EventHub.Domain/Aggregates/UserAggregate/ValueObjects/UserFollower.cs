using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.UserAggregate.ValueObjects;

[Table("UserFollowers")]
public class UserFollower : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required Guid FollowerId { get; set; } = Guid.Empty;

    [Required]
    public required Guid FollowedId { get; set; } = Guid.Empty;

    [ForeignKey("FollowerId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Follower { get; set; } = null!;

    [ForeignKey("FollowedId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Followed { get; set; } = null!;
}
