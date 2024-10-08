﻿using EventHub.Shared.DTOs.Review;
using FluentValidation;

namespace EventHub.Shared.Validators.Review;

public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .WithMessage("Content cannot over limit 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Content));

        RuleFor(x => x.Rate).NotEmpty().WithMessage("Rate is required")
            .InclusiveBetween(0.0, 5.0)
            .WithMessage("Rate must be in range from 0.0 to 5.0");
    }
}