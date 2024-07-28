using EventHub.Domain.DTOs.Event;
using FluentValidation;

namespace EventHub.Domain.Validators.Event;

public class CreateFavouriteEventDtoValidator : AbstractValidator<CreateFavouriteEventDto>
{
    public CreateFavouriteEventDtoValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("Event's id is required")
            .MaximumLength(50).WithMessage("Event's id cannot over limit 50 characters");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required")
            .MaximumLength(50).WithMessage("User's id cannot over limit 50 characters");
    }
}