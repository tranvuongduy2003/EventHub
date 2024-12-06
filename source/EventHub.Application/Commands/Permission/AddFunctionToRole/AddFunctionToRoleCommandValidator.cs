using FluentValidation;

namespace EventHub.Application.Commands.Permission.AddFunctionToRole;

public class AddFunctionToRoleCommandValidator : AbstractValidator<AddFunctionToRoleCommand>
{
    public AddFunctionToRoleCommandValidator()
    {
        RuleFor(x => x.FunctionId)
            .NotEmpty()
            .WithMessage("Function ID is required")
            .MaximumLength(50)
            .WithMessage("Function ID cannot exceed 50 characters");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role ID is required");
    }
}
