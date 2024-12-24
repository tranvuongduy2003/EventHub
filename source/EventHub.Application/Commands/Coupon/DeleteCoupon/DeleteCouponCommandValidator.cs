using FluentValidation;

namespace EventHub.Application.Commands.Coupon.DeleteCoupon;

public class DeleteCouponCommandValidator : AbstractValidator<DeleteCouponCommand>
{
    public DeleteCouponCommandValidator()
    {
        RuleFor(x => x.CouponId.ToString())
            .NotEmpty()
            .WithMessage("Coupon ID is required")
            .MaximumLength(50)
            .WithMessage("Coupon ID cannot exceed 50 characters");
    }
}
