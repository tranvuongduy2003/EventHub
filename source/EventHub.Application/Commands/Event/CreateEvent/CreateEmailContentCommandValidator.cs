using FluentValidation;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateEmailContentCommandValidator : AbstractValidator<CreateEmailContentCommand>
{
    public CreateEmailContentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Email content is required")
            .MaximumLength(4000).WithMessage("Email content cannot over 4000 characters limit");
    }
}
