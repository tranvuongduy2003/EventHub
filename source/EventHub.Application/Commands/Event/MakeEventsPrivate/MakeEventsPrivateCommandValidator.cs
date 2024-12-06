using FluentValidation;

namespace EventHub.Application.Commands.Event.MakeEventsPrivate;

public class MakeEventsPrivateCommandValidator : AbstractValidator<MakeEventsPrivateCommand>
{
    public MakeEventsPrivateCommandValidator()
    {
        RuleFor(x => x.UserId.ToString())
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.Events)
            .NotEmpty()
            .WithMessage("At least one event must be specified");
    }
}
