using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.CouponAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;

[Table("PaymentCoupons")]
[PrimaryKey("PaymentId", "CouponId")]
public class PaymentCoupon : EntityBase
{
    [Required]
    public required Guid PaymentId { get; set; } = Guid.Empty;

    [Required]
    public required Guid CouponId { get; set; } = Guid.Empty;

    [ForeignKey("PaymentId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Payment Payment { get; set; } = null!;

    [ForeignKey("CouponId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual Coupon Coupon { get; set; } = null!;
}
