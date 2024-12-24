using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

namespace EventHub.Application.Queries.Coupon.GetCreatedCouponsByUserId;

public record GetCreatedCouponsByUserIdQuery(PaginationFilter Filter) : IQuery<Pagination<CouponDto>>;
