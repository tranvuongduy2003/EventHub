using EventHub.Domain.AggregateModels.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

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