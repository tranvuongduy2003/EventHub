using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Coupon.GetCouponById;

public class GetCouponByIdQueryHandler : IQueryHandler<GetCouponByIdQuery, CouponDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCouponByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CouponDto> Handle(GetCouponByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons
            .FindByCondition(x => x.Id == request.Id)
            .Include(x => x.EventCoupons)
            .ThenInclude(x => x.Event)
            .FirstOrDefaultAsync(cancellationToken);
        
        return _mapper.Map<CouponDto>(coupon);
    }
}
