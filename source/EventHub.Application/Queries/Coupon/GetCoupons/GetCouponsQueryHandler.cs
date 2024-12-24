using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Coupon.GetCoupons;

public class GetCouponsQueryHandler : IQueryHandler<GetCouponsQuery, Pagination<CouponDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<CouponDto>> Handle(GetCouponsQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.CouponAggregate.Coupon> paginatedCoupons = _unitOfWork.Coupons
            .PaginatedFind(request.Filter, query => query
                .Include(x => x.EventCoupons)
                .ThenInclude(x => x.Event));

        Pagination<CouponDto> paginatedCouponDtos = _mapper.Map<Pagination<CouponDto>>(paginatedCoupons);

        return Task.FromResult(paginatedCouponDtos);
    }
}
