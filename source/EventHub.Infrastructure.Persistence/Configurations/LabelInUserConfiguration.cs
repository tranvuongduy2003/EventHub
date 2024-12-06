using EventHub.Domain.Aggregates.LabelAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class LabelInUserConfiguration: IEntityTypeConfiguration<LabelInUser>
{
    public void Configure(EntityTypeBuilder<LabelInUser> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.LabelInUsers)
            .HasForeignKey(x => x.UserId);
        
        builder
            .HasOne(x => x.Label)
            .WithMany(x => x.LabelInUsers)
            .HasForeignKey(x => x.LabelId);
    }
}