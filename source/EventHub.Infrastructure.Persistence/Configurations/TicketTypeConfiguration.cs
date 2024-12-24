using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.EventAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class TicketTypeConfiguration: IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder
            .HasMany(x => x.PaymentItems)
            .WithOne(x => x.TicketType)
            .HasForeignKey(x => x.TicketTypeId);
        
        builder
            .HasMany(x => x.Tickets)
            .WithOne(x => x.TicketType)
            .HasForeignKey(x => x.TicketTypeId);
    }
}