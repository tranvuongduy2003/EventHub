using FluentValidation;

namespace EventHub.Application.Commands.Event.MakeEventsPublic;

public class MakeEventsPublicCommandValidator : AbstractValidator<MakeEventsPublicCommand>
{
    public MakeEventsPublicCommandValidator()
    {
        RuleFor(x => x.Events)
            .NotEmpty()
            .WithMessage("At least one event must be specified");
    }
}
