using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;

[Table("EventCoupons")]
[PrimaryKey("CouponId", "EventId")]
public class EventCoupon : EntityBase
{
    [Required] public required Guid CouponId { get; set; } = Guid.Empty;

    [Required] public required Guid EventId { get; set; } = Guid.Empty;

    [ForeignKey("CouponId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Coupon Coupon { get; set; } = null!;

    [ForeignKey("EventId")]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public virtual Event Event { get; set; } = null!;
}
