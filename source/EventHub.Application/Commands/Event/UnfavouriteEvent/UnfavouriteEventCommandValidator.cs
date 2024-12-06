using FluentValidation;

namespace EventHub.Application.Commands.Event.UnfavouriteEvent;

public class UnfavouriteEventCommandValidator : AbstractValidator<UnfavouriteEventCommand>
{
    public UnfavouriteEventCommandValidator()
    {
        RuleFor(x => x.UserId.ToString())
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.EventId.ToString())
            .NotEmpty()
            .WithMessage("Event ID is required");
    }
}
