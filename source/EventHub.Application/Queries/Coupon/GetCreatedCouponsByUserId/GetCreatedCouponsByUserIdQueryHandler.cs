using AutoMapper;
using EventHub.Application.Queries.Coupon.GetCoupons;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Coupon.GetCreatedCouponsByUserId;

public class
    GetCreatedCouponsByUserIdQueryHandler : IQueryHandler<GetCreatedCouponsByUserIdQuery, Pagination<CouponDto>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetCreatedCouponsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public Task<Pagination<CouponDto>> Handle(GetCreatedCouponsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

        Pagination<Domain.Aggregates.CouponAggregate.Coupon> paginatedCoupons = _unitOfWork.Coupons
            .PaginatedFindByCondition(
                x => x.AuthorId == userId,
                request.Filter,
                query => query
                    .Include(x => x.EventCoupons)
                    .ThenInclude(x => x.Event));

        Pagination<CouponDto> paginatedCouponDtos = _mapper.Map<Pagination<CouponDto>>(paginatedCoupons);

        return Task.FromResult(paginatedCouponDtos);
    }
}
