using FluentValidation;

namespace EventHub.Application.Commands.Event.PermanentlyDeleteEvent;

public class PermanentlyDeleteEventCommandValidator : AbstractValidator<PermanentlyDeleteEventCommand>
{
    public PermanentlyDeleteEventCommandValidator()
    {
        RuleFor(x => x.EventId.ToString())
            .NotEmpty()
            .WithMessage("Event ID is required");
    }
}
