using EventHub.Domain.Enums.Payment;
using EventHub.Domain.DTOs.Payment;
using FluentValidation;

namespace EventHub.Domain.Validators.Payment;

public class UpdatePaymentStatusDtoValidator : AbstractValidator<UpdatePaymentStatusDto>
{
    public UpdatePaymentStatusDtoValidator()
    {
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required")
            .Must(status => status == EPaymentStatus.PAID || status == EPaymentStatus.PENDING ||
                            status == EPaymentStatus.FAILED || status == EPaymentStatus.REJECTED)
            .WithMessage("Status is invalid");
    }
}