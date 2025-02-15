using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class InvitationConfiguration: IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder
            .HasOne(x => x.Inviter)
            .WithMany(x => x.Inviters)
            .HasForeignKey(x => x.InviterId);
        
        builder
            .HasOne(x => x.Invited)
            .WithMany(x => x.Inviteds)
            .HasForeignKey(x => x.InvitedId);
    }
}