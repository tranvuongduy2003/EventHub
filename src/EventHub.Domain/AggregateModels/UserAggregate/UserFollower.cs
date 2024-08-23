﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.AggregateModels.UserAggregate;

[Table("UserFollowers")]
[PrimaryKey("FollowerId", "FollowedId")]
public class UserFollower : EntityBase
{
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public Guid FollowerId { get; set; } = Guid.Empty;

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public Guid FollowedId { get; set; } = Guid.Empty;

    [ForeignKey("FollowerId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Follower { get; set; } = null!;

    [ForeignKey("FollowedId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Followed { get; set; } = null!;
}