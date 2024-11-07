using EventHub.Domain.AggregateModels.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Persistence.Configurations;

public class UserPaymentMethodConfiguration: IEntityTypeConfiguration<UserPaymentMethod>
{
    public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
    {
        builder
            .HasMany(x => x.Payments)
            .WithOne(x => x.UserPaymentMethod)
            .HasForeignKey(x => x.UserPaymentMethodId);

    }
}