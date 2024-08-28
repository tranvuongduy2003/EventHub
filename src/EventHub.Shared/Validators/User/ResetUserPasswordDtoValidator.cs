using EventHub.Shared.DTOs.User;
using FluentValidation;

namespace EventHub.Shared.Validators.User;

public class ResetUserPasswordDtoValidator : AbstractValidator<ResetUserPasswordDto>
{
    public ResetUserPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid format");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Old password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("Password has to at least 8 characters")
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password is not match complexity rules.");
    }
}