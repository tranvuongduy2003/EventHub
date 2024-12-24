using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class FavouriteEventConfiguration: IEntityTypeConfiguration<FavouriteEvent>
{
    public void Configure(EntityTypeBuilder<FavouriteEvent> builder)
    {
        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.FavouriteEvents)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.FavouriteEvents)
            .HasForeignKey(x => x.UserId);
    }
}