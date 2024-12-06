using FluentValidation;

namespace EventHub.Application.Commands.Function.DeleteFunction;

public class DeleteFunctionCommandValidator : AbstractValidator<DeleteFunctionCommand>
{
    public DeleteFunctionCommandValidator()
    {
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("Function ID is required")
            .MaximumLength(50)
            .WithMessage("Function ID cannot exceed 50 characters");
    }
}
