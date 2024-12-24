using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Coupon.CreateCoupon;

public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, CouponDto>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task<CouponDto> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");


        var coupon = new Domain.Aggregates.CouponAggregate.Coupon
        {
            Name = request.Name,
            Description = request.Description,
            Quantity = request.MinQuantity,
            MinPrice = request.MinPrice,
            PercentValue = request.PercentValue,
            ExpiredDate = request.ExpiredDate,
            AuthorId = authorId
        };

        await _unitOfWork.Coupons.CreateAsync(coupon);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CouponDto>(coupon);
    }
}
