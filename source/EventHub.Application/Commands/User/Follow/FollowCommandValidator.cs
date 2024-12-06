using FluentValidation;

namespace EventHub.Application.Commands.User.Follow;

public class FollowCommandValidator : AbstractValidator<FollowCommand>
{
    public FollowCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required");

        RuleFor(x => x.FollowedUserId.ToString())
            .NotEmpty()
            .WithMessage("Followed user ID is required");
    }
}
