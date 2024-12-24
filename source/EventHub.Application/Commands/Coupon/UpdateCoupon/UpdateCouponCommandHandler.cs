using AutoMapper;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Coupon.UpdateCoupon;

public class UpdateCouponCommandHandler : ICommandHandler<UpdateCouponCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.CouponAggregate.Coupon coupon = await _unitOfWork.Coupons.GetByIdAsync(request.Id);
        if (coupon is null)
        {
            throw new NotFoundException("Coupon does not exist!");
        }

        coupon = _mapper.Map<Domain.Aggregates.CouponAggregate.Coupon>(request);

        await _unitOfWork.Coupons.Update(coupon);
        await _unitOfWork.CommitAsync();
    }
}
