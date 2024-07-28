using EventHub.Domain.DTOs.Role;
using FluentValidation;

namespace EventHub.Domain.Validators.Role;

public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id value is required").MaximumLength(50)
            .WithMessage("Role id cannot over limit 50 characters");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required");
    }
}