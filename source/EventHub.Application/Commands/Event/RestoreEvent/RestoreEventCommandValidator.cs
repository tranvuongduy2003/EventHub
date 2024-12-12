using FluentValidation;

namespace EventHub.Application.Commands.Event.RestoreEvent;

public class RestoreEventCommandValidator : AbstractValidator<RestoreEventCommand>
{
    public RestoreEventCommandValidator()
    {
        RuleFor(x => x.Events)
            .NotEmpty()
            .WithMessage("At least one event must be specified");
    }
}
