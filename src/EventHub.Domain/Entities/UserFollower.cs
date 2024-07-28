using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Entities;

[Table("UserFollowers")]
[PrimaryKey("FollowerId", "FollowedId")]
public class UserFollower : EntityBase
{
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public string FollowerId { get; set; } = string.Empty;

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public string FollowedId { get; set; } = string.Empty;

    [ForeignKey("FollowerId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Follower { get; set; } = null!;

    [ForeignKey("FollowedId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Followed { get; set; } = null!;
}