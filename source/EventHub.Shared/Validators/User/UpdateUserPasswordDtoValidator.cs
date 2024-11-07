using EventHub.Shared.DTOs.User;
using FluentValidation;

namespace EventHub.Shared.Validators.User;

public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDto>
{
    public UpdateUserPasswordDtoValidator()
    {
        RuleFor(x => x.UserId.ToString())
            .NotEmpty().WithMessage("AuthorId is required")
            .MaximumLength(50).WithMessage("AuthorId cannot over limit 50 characters");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("Password has to at least 8 characters")
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password is not match complexity rules.");
    }
}