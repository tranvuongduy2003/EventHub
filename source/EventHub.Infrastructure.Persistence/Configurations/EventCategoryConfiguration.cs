using EventHub.Domain.Aggregates.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class EventCategoryConfiguration: IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}