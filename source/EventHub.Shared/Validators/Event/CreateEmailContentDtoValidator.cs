using EventHub.Shared.DTOs.Event;
using FluentValidation;

namespace EventHub.Shared.Validators.Event;

public class CreateEmailContentDtoValidator : AbstractValidator<CreateEmailContentDto>
{
    public CreateEmailContentDtoValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Email content is required")
            .MaximumLength(4000).WithMessage("Email content cannot over 4000 characters limit");
    }
}
