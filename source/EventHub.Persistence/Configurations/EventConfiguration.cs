using EventHub.Domain.AggregateModels.EventAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class EventConfiguration: IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .HasMany(x => x.EventSubImages)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.TicketTypes)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Invitations)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Payments)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Reviews)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Tickets)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Conversations)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Messages)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
        
        builder
            .HasMany(x => x.Reasons)
            .WithOne(x => x.Event)
            .HasForeignKey(x => x.EventId);
    }
}