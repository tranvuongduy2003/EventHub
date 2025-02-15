﻿using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Coupon;

public class EventCouponDto
{
    [SwaggerSchema("Unique identifier for the coupon")]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public float PercentValue { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public long MinPrice { get; set; }

    public DateTime ExpiredDate { get; set; }
}
