using FluentValidation;

namespace EventHub.Application.Commands.Permission.EnableCommandInFunction;

public class EnableCommandInFunctionCommandValidator : AbstractValidator<EnableCommandInFunctionCommand>
{
    public EnableCommandInFunctionCommandValidator()
    {
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("Function ID is required")
            .MaximumLength(50)
            .WithMessage("Function ID cannot exceed 50 characters");

        RuleFor(x => x.CommandId)
            .NotEmpty()
            .WithMessage("Command ID is required")
            .MaximumLength(50)
            .WithMessage("Command ID cannot exceed 50 characters");
    }
}
