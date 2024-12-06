using EventHub.Application.DTOs.Function;
using EventHub.Domain.SeedWork.Command;
using FluentValidation;

namespace EventHub.Application.Commands.Function.CreateFunction;

public class CreateFunctionCommandValidator : AbstractValidator<CreateFunctionCommand>
{
    public CreateFunctionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Function name is required")
            .MaximumLength(200)
            .WithMessage("Function name cannot exceed 200 characters");

        RuleFor(x => x.Url.ToString())
            .NotEmpty()
            .WithMessage("Function URL is required")
            .MaximumLength(200)
            .WithMessage("Function URL cannot exceed 200 characters");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sort order must be greater than or equal to 0");

        When(x => !string.IsNullOrEmpty(x.ParentId), () =>
        {
            RuleFor(x => x.ParentId)
                .MaximumLength(50)
                .WithMessage("Parent ID cannot exceed 50 characters");
        });
    }
}
