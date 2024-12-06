using FluentValidation;

namespace EventHub.Application.Commands.Event.UpdateEvent;

public class UpdateEmailContentCommandValidator : AbstractValidator<UpdateEmailContentCommand>
{
    public UpdateEmailContentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Email content ID is required");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Email content is required")
            .MaximumLength(4000).WithMessage("Email content cannot over 4000 characters limit");
    }
}
