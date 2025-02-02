using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventHub.Infrastructure.Persistence.Configurations;

public class PaymentCouponConfiguration : IEntityTypeConfiguration<PaymentCoupon>
{
    public void Configure(EntityTypeBuilder<PaymentCoupon> builder)
    {
        builder
            .HasOne(x => x.Payment)
            .WithMany(x => x.PaymentCoupons)
            .HasForeignKey(x => x.PaymentId);

        builder
            .HasOne(x => x.Coupon)
            .WithMany(x => x.PaymentCoupons)
            .HasForeignKey(x => x.CouponId);
    }
}
