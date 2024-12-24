using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Coupon.DeleteCoupon;

public class DeleteCouponCommandHandler : ICommandHandler<DeleteCouponCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCouponCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons.GetByIdAsync(request.CouponId);
        if (coupon is null)
        {
            throw new NotFoundException("Coupon does not exist!");
        }

        await _unitOfWork.Coupons.Delete(coupon);
        await _unitOfWork.CommitAsync();
    }
}
