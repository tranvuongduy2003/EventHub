using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.CouponAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Event.ApplyCoupons;

public class ApplyCouponsCommandHandler : ICommandHandler<ApplyCouponsCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApplyCouponsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ApplyCouponsCommand request, CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        foreach (Guid couponId in request.CouponIds)
        {
            Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons.GetByIdAsync(couponId);
            if (coupon != null)
            {
                var eventCoupon = new EventCoupon
                {
                    CouponId = couponId,
                    EventId = request.EventId,
                };
                await _unitOfWork.EventCoupons.CreateAsync(eventCoupon);
                coupon.Quantity--;
                await _unitOfWork.Coupons.Update(coupon);
            }
        }
        
        await _unitOfWork.CommitAsync();
    }
}
