using EventHub.Domain.DTOs.Review;
using FluentValidation;

namespace EventHub.Domain.Validators.Review;

public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("Event's id is required")
            .MaximumLength(50).WithMessage("Event's id cannot over limit 50 characters");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required")
            .MaximumLength(50).WithMessage("User's id cannot over limit 50 characters");

        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .WithMessage("Content cannot over limit 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Content));

        RuleFor(x => x.Rate).NotEmpty().WithMessage("Rate is required")
            .InclusiveBetween(0.0, 5.0)
            .WithMessage("Rate must be in range from 0.0 to 5.0");
    }
}