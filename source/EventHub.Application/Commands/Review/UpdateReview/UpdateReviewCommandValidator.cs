using FluentValidation;

namespace EventHub.Application.Commands.Review.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Review ID is required");

        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .WithMessage("Review content cannot exceed 1000 characters");

        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");
    }
}
