using FluentValidation;

namespace EventHub.Application.Commands.Auth.ValidateUser;

public class ValidateUserCommandValidator : AbstractValidator<ValidateUserCommand>
{
    public ValidateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not match");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(255).WithMessage("Fullname cannot over 255 characters limit");
    }
}
