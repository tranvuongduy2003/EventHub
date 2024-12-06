using FluentValidation;

namespace EventHub.Application.Commands.Permission.RemoveFunctionFromRole;

public class RemoveFunctionFromRoleCommandValidator : AbstractValidator<RemoveFunctionFromRoleCommand>
{
    public RemoveFunctionFromRoleCommandValidator()
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
