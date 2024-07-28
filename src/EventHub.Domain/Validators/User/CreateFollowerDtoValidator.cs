using EventHub.Domain.DTOs.User;
using FluentValidation;

namespace EventHub.Domain.Validators.User;

public class CreateFollowerDtoValidator : AbstractValidator<CreateFollowerDto>
{
    public CreateFollowerDtoValidator()
    {
        RuleFor(x => x.FollowerId).NotEmpty().WithMessage("Follower's id is required")
            .MaximumLength(50).WithMessage("Follower's id cannot over limit 50 characters");

        RuleFor(x => x.FollowedId).NotEmpty().WithMessage("Followed's id is required")
            .MaximumLength(50).WithMessage("Followed's id cannot over limit 50 characters");
    }
}