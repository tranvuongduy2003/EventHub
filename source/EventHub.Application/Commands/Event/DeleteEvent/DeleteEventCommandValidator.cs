using FluentValidation;

namespace EventHub.Application.Commands.Event.DeleteEvent;

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.EventId.ToString())
            .NotEmpty()
            .WithMessage("Event ID is required");
    }
}
