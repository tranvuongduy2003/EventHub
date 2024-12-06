using EventHub.Domain.Aggregates.LabelAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class LabelInEventConfiguration: IEntityTypeConfiguration<LabelInEvent>
{
    public void Configure(EntityTypeBuilder<LabelInEvent> builder)
    {
        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.LabelInEvents)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasOne(x => x.Label)
            .WithMany(x => x.LabelInEvents)
            .HasForeignKey(x => x.LabelId);
    }
}