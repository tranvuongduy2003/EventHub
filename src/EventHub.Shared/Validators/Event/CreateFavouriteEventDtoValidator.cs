using EventHub.Shared.DTOs.Event;
using FluentValidation;

namespace EventHub.Shared.Validators.Event;

public class CreateFavouriteEventDtoValidator : AbstractValidator<CreateFavouriteEventDto>
{
    public CreateFavouriteEventDtoValidator()
    {
        RuleFor(x => x.EventId.ToString())
            .NotEmpty().WithMessage("Event's id is required")
            .MaximumLength(50).WithMessage("Event's id cannot over limit 50 characters");

        RuleFor(x => x.UserId.ToString())
            .NotEmpty().WithMessage("User's id is required")
            .MaximumLength(50).WithMessage("User's id cannot over limit 50 characters");
    }
}