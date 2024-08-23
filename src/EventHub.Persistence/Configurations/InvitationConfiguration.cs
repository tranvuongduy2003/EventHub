using EventHub.Domain.AggregateModels.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

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