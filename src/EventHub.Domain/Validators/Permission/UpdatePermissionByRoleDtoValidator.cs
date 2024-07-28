using EventHub.Domain.DTOs.Permission;
using FluentValidation;

namespace EventHub.Domain.Validators.Permission;

public class UpdatePermissionByRoleDtoValidator : AbstractValidator<UpdatePermissionByRoleDto>
{
    public UpdatePermissionByRoleDtoValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty().WithMessage("RoleId is required");

        RuleFor(x => x.FunctionId).NotEmpty().WithMessage("FunctionId is required");

        RuleFor(x => x.Value)
            .NotNull()
            .WithMessage("Value is required");
    }
}