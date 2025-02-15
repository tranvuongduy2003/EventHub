using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class UserFollowerConfiguration: IEntityTypeConfiguration<UserFollower>
{
    public void Configure(EntityTypeBuilder<UserFollower> builder)
    {
        builder
            .HasOne(x => x.Followed)
            .WithMany(x => x.Followeds)
            .HasForeignKey(x => x.FollowedId);
        
        builder
            .HasOne(x => x.Follower)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowerId);
    }
}