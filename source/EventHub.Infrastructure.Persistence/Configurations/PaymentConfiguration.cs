using EventHub.Domain.Aggregates.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration: IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .HasMany(x => x.Tickets)
            .WithOne(x => x.Payment)
            .HasForeignKey(x => x.PaymentId);
        
        builder
            .HasMany(x => x.PaymentItems)
            .WithOne(x => x.Payment)
            .HasForeignKey(x => x.PaymentId);
    }
}