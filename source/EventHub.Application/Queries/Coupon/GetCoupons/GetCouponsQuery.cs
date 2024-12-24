using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Coupon.GetCoupons;

/// <summary>
/// Represents a query to retrieve a list of all coupons.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of all available coupons without any specific filtering or pagination.
/// </remarks>
public record GetCouponsQuery(PaginationFilter Filter) : IQuery<Pagination<CouponDto>>;
