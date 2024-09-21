using EventHub.Shared.DTOs.Conversation;
using FluentValidation;

namespace EventHub.Shared.Validators.Conversation;

public class JoinChatRoomDtoValidator : AbstractValidator<JoinChatRoomDto>
{
    public JoinChatRoomDtoValidator()
    {
        RuleFor(x => x.EventId.ToString())
            .NotEmpty().WithMessage("EventId is required")
            .MaximumLength(50).WithMessage("EventId cannot over limit 50 characters");

        RuleFor(x => x.HostId.ToString())
            .NotEmpty().WithMessage("HostId is required")
            .MaximumLength(50).WithMessage("HostId cannot over limit 50 characters");

        RuleFor(x => x.UserId.ToString())
            .NotEmpty().WithMessage("AuthorId is required")
            .MaximumLength(50).WithMessage("AuthorId cannot over limit 50 characters");
    }
}