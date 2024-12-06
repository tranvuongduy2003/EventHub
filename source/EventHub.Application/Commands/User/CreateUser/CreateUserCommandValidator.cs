using FluentValidation;

namespace EventHub.Application.Commands.User.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .MinimumLength(2)
            .WithMessage("Full name must be at least 2 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .Must(password => password.Any(char.IsUpper))
            .WithMessage("Password must contain at least one uppercase letter")
            .Must(password => password.Any(char.IsLower))
            .WithMessage("Password must contain at least one lowercase letter")
            .Must(password => password.Any(char.IsDigit))
            .WithMessage("Password must contain at least one number");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters");

        RuleFor(x => x.Dob)
            .Must(dob => !dob.HasValue || dob.Value < DateTime.UtcNow)
            .WithMessage("Date of birth cannot be in the future");
    }
}
