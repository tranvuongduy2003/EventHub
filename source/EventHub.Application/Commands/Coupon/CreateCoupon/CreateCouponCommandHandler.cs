using AutoMapper;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Application.SeedWork.DTOs.File;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Stripe;

namespace EventHub.Application.Commands.Coupon.CreateCoupon;

public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, CouponDto>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
        _fileService = fileService;
    }

    public async Task<CouponDto> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");


        var coupon = new Domain.Aggregates.CouponAggregate.Coupon
        {
            Name = request.Name,
            Description = request.Description,
            Quantity = request.Quantity,
            MinPrice = request.MinPrice,
            PercentValue = request.PercentValue,
            ExpiredDate = request.ExpiredDate,
            AuthorId = authorId
        };

        if (request.CoverImage != null)
        {
            BlobResponseDto coverImage = await _fileService.UploadAsync(request.CoverImage, FileContainer.COUPONS);
            coupon.CoverImageUrl = coverImage.Blob.Uri!;
            coupon.CoverImageFileName = coverImage.Blob.Name!;
        }

        var couponOptions = new CouponCreateOptions
        {
            Duration = "once",
            PercentOff = (decimal)coupon.PercentValue,
            Currency = "vnd",
            Name = coupon.Name,
            RedeemBy = coupon.ExpiredDate
        };

        var service = new CouponService();
        Stripe.Coupon stripeCoupon = await service.CreateAsync(couponOptions, cancellationToken: cancellationToken);

        coupon.Code = stripeCoupon.Id;

        await _unitOfWork.Coupons.CreateAsync(coupon);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CouponDto>(coupon);
    }
}
