using FluentValidation;

namespace EventHub.Application.Commands.User.Unfollow;

public class UnfollowCommandValidator : AbstractValidator<UnfollowCommand>
{
    public UnfollowCommandValidator()
    {
        RuleFor(x => x.FollowedUserId.ToString())
            .NotEmpty()
            .WithMessage("Unfollowed user ID is required");
    }
}
