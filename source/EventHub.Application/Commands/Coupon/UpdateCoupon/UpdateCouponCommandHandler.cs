using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.File;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;

namespace EventHub.Application.Commands.Coupon.UpdateCoupon;

public class UpdateCouponCommandHandler : ICommandHandler<UpdateCouponCommand>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCouponCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons.GetByIdAsync(request.Id);
        if (coupon is null)
        {
            throw new NotFoundException("Coupon does not exist!");
        }

        coupon.Name = request.Name;
        coupon.Description = request.Description;
        coupon.Quantity = request.Quantity;
        coupon.MinPrice = request.MinPrice;
        coupon.PercentValue = request.PercentValue;
        coupon.ExpiredDate = request.ExpiredDate;

        if (request.CoverImage != null)
        {
            if (!string.IsNullOrEmpty(coupon.CoverImageFileName))
            {
                await _fileService.DeleteAsync(coupon.CoverImageFileName, FileContainer.COUPONS);
            }
            BlobResponseDto coverImage = await _fileService.UploadAsync(request.CoverImage, FileContainer.COUPONS);
            coupon.CoverImageUrl = coverImage.Blob.Uri!;
            coupon.CoverImageFileName = coverImage.Blob.Name!;
        }

        await _unitOfWork.Coupons.Update(coupon);
        await _unitOfWork.CommitAsync();
    }
}
