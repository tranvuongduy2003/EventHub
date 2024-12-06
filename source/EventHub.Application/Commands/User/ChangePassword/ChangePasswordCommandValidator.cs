using FluentValidation;

namespace EventHub.Application.Commands.User.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId.ToString())
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .Must(password => password.Any(char.IsUpper))
            .WithMessage("Password must contain at least one uppercase letter")
            .Must(password => password.Any(char.IsLower))
            .WithMessage("Password must contain at least one lowercase letter")
            .Must(password => password.Any(char.IsDigit))
            .WithMessage("Password must contain at least one number");

        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .WithMessage("OldPassword is required");
    }
}
