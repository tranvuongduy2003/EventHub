using EventHub.Domain.DTOs.User;
using FluentValidation;

namespace EventHub.Domain.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        //RuleFor(x => x.FullName).NotEmpty().WithMessage("User name is required");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password has to atleast 8 characters")
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password is not match complexity rules.");

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