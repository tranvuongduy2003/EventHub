using EventHub.Domain.AggregateModels.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class PaymentMethodConfiguration: IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder
            .HasMany(x => x.UserPaymentMethods)
            .WithOne(x => x.PaymentMethod)
            .HasForeignKey(x => x.PaymentMethodId);
    }
}