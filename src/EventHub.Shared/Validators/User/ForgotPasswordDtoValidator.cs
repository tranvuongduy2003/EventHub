using EventHub.Shared.DTOs.Auth;
using FluentValidation;

namespace EventHub.Shared.Validators.User;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid format");
    }
}