using FluentValidation;

namespace EventHub.Application.Commands.Event.FavouriteEvent;

public class FavouriteEventCommandValidator : AbstractValidator<FavouriteEventCommand>
{
    public FavouriteEventCommandValidator()
    {
        RuleFor(x => x.EventId.ToString())
            .NotEmpty()
            .WithMessage("Event ID is required");
    }
}
