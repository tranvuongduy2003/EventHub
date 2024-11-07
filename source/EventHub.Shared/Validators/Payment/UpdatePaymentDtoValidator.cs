using EventHub.Shared.DTOs.Payment;
using FluentValidation;

namespace EventHub.Shared.Validators.Payment;

public class UpdatePaymentDtoValidator : AbstractValidator<UpdatePaymentDto>
{
    public UpdatePaymentDtoValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("CustomerName is required")
            .MaximumLength(100).WithMessage("CustomerName cannot over limit 100 characters");

        RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage("CustomerPhone is required")
            .MaximumLength(100).WithMessage("CustomerPhone cannot over limit 100 characters");

        RuleFor(x => x.CustomerEmail).NotEmpty().WithMessage("CustomerEmail is required")
            .EmailAddress().WithMessage("CustomerEmail is invalid format")
            .MaximumLength(100).WithMessage("CustomerEmail cannot over limit 100 characters");
    }
}