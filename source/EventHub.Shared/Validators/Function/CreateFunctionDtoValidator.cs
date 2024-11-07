using EventHub.Shared.DTOs.Function;
using FluentValidation;

namespace EventHub.Shared.Validators.Function;

public class CreateFunctionDtoValidator : AbstractValidator<CreateFunctionDto>
{
    public CreateFunctionDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name value is required")
            .MaximumLength(200).WithMessage("Name cannot over limit 200 characters");

        RuleFor(x => x.Url).NotEmpty().WithMessage("URL value is required")
            .MaximumLength(200).WithMessage("URL cannot over limit 200 characters");

        RuleFor(x => x.ParentId).MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.ParentId))
            .WithMessage("ParentId cannot over limit 50 characters");
    }
}