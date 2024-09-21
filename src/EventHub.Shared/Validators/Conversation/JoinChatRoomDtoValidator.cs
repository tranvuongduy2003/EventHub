﻿using EventHub.Shared.DTOs.Conversation;
using FluentValidation;

namespace EventHub.Shared.Validators.Conversation;

public class JoinChatRoomDtoValidator : AbstractValidator<JoinChatRoomDto>
{
    public JoinChatRoomDtoValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("EventId is required")
            .MaximumLength(50).WithMessage("EventId cannot over limit 50 characters");

        RuleFor(x => x.HostId).NotEmpty().WithMessage("HostId is required")
            .MaximumLength(50).WithMessage("HostId cannot over limit 50 characters");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("AuthorId is required")
            .MaximumLength(50).WithMessage("AuthorId cannot over limit 50 characters");
    }
}