using FluentValidation;

namespace EventHub.Application.Commands.Coupon.CreateCoupon;

public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Coupon name is required")
            .MaximumLength(100)
            .WithMessage("Coupon name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.MinQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum quantity must be greater than or equal to 0");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum price must be greater than or equal to 0");

        RuleFor(x => x.PercentValue)
            .InclusiveBetween(0, 100)
            .WithMessage("Percent value must be between 0 and 100");

        RuleFor(x => x.ExpiredDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future");
    }
}
