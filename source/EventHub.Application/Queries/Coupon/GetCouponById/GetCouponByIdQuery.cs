using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Coupon.GetCouponById;

/// <summary>
/// Represents a query to retrieve a coupon's details by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of a coupon's information based on its unique identifier.
/// </remarks>
public record GetCouponByIdQuery(Guid Id) : IQuery<CouponDto>;
