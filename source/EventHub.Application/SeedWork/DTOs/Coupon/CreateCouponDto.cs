﻿using Microsoft.AspNetCore.Http;

namespace EventHub.Application.SeedWork.DTOs.Coupon;

public class CreateCouponDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public long MinPrice { get; set; }

    public int PercentValue { get; set; }

    public DateTime ExpiredDate { get; set; }

    public IFormFile CoverImage { get; set; }
}
