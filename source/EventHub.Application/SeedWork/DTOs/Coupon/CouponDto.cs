using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Coupon;

public class CouponDto
{
    [SwaggerSchema("Unique identifier for the coupon")]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public long MinPrice { get; set; }

    public float PercentValue { get; set; }

    public DateTime ExpiredDate { get; set; }

    public string CoverImage { get; set; }

    [SwaggerSchema("Creation timestamp of the coupon")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [SwaggerSchema("Timestamp of the last update to the coupon")]
    public DateTime? UpdatedAt { get; set; } = null;
}
