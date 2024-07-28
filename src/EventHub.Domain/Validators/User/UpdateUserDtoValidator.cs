using EventHub.Domain.DTOs.User;
using FluentValidation;

namespace EventHub.Domain.Validators.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not match");

        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");

        RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required")
            .MaximumLength(50).WithMessage("Full name cannot over 50 characters limit");

        RuleFor(x => x.Bio)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Bio))
            .WithMessage("Bio cannot over 1000 characters limit");
    }
}