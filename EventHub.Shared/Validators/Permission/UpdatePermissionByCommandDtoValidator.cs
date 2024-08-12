using EventHub.Shared.DTOs.Permission;
using FluentValidation;

namespace EventHub.Shared.Validators.Permission;

public class UpdatePermissionByCommandDtoValidator : AbstractValidator<UpdatePermissionByCommandDto>
{
    public UpdatePermissionByCommandDtoValidator()
    {
        RuleFor(x => x.CommandId).NotEmpty().WithMessage("CommandId is required");

        RuleFor(x => x.FunctionId).NotEmpty().WithMessage("FunctionId is required");

        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value is required");
    }
}