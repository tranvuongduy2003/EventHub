using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;
using EventHub.Domain.Aggregates.PaymentAggregate;
using EventHub.Domain.Aggregates.PaymentAggregate.ValueObjects;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.AggregateRoot;
using EventHub.Domain.SeedWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Domain.Aggregates.CouponAggregate;

[Table("Coupons")]
public class Coupon : AggregateRoot, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public long MinPrice { get; set; }

    [Range(0, 100)]
    public int PercentValue { get; set; }

    public string Code { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string CoverImageUrl { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string CoverImageFileName { get; set; } = string.Empty;

    public DateTime ExpiredDate { get; set; }

    public Guid AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    public virtual User Author { get; set; } = null!;

    public virtual ICollection<EventCoupon> EventCoupons { get; set; } = new List<EventCoupon>();

    public virtual ICollection<PaymentCoupon> PaymentCoupons { get; set; } = new List<PaymentCoupon>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
