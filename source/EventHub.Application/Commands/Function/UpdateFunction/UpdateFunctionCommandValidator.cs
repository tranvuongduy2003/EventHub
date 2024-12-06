using FluentValidation;

namespace EventHub.Application.Commands.Function.UpdateFunction;

public class UpdateFunctionCommandValidator : AbstractValidator<UpdateFunctionCommand>
{
    public UpdateFunctionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Function ID is required")
            .MaximumLength(50)
            .WithMessage("Function ID cannot exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Url.ToString())
            .NotEmpty()
            .WithMessage("URL is required")
            .MaximumLength(500)
            .WithMessage("URL cannot exceed 500 characters");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sort order must be greater than or equal to 0");

        When(x => !string.IsNullOrEmpty(x.ParentId), () =>
        {
            RuleFor(x => x.ParentId)
                .MaximumLength(50)
                .WithMessage("Parent function ID cannot exceed 50 characters");
        });
    }
}
