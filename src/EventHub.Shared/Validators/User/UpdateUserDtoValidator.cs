using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.User;
using FluentValidation;

namespace EventHub.Shared.Validators.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Id.ToString())
            .NotEmpty().WithMessage("Id is required")
            .MaximumLength(50).WithMessage("Id cannot over limit 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not match");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required");

        RuleFor(x => x.Dob)
            .LessThan(DateTime.UtcNow)
            .When(x => x.Dob != null)
            .NotEmpty().WithMessage("Date of birth must be before current date");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(255).WithMessage("Fullname cannot over 255 characters limit");

        RuleFor(x => x.UserName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.UserName))
            .WithMessage("Username cannot over 50 characters limit");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender != null)
            .WithMessage($"Gender must be {nameof(EGender.MALE)}, {nameof(EGender.FEMALE)} or {nameof(EGender.OTHER)}");

        RuleFor(x => x.Bio)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Bio))
            .WithMessage("Bio cannot over 1000 characters limit");
    }
}