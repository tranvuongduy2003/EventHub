using FluentValidation;

namespace EventHub.Application.Commands.Review.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithMessage("Author ID is required");

        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .WithMessage("Review content cannot exceed 1000 characters");

        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");
    }
}
