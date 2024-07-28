using EventHub.Domain.DTOs.Message;
using FluentValidation;

namespace EventHub.Domain.Validators.Message;

public class SendMessageDtoValidator : AbstractValidator<SendMessageDto>
{
    public SendMessageDtoValidator()
    {
        RuleFor(x => x.ConversationId).NotEmpty().WithMessage("ConversationId is required")
            .MaximumLength(50).WithMessage("ConversationId cannot over limit 50 characters");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required")
            .MaximumLength(50).WithMessage("UserId cannot over limit 50 characters");

        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Content))
            .WithMessage("Content cannot over limit 1000 characters");

        RuleFor(x => x.ImageId)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.ImageId))
            .WithMessage("ImageId cannot over limit 50 characters");

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.ImageId))
            .WithMessage("ImageUrl is required");

        RuleFor(x => x.VideoId)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.VideoId))
            .WithMessage("VideoId cannot over limit 50 characters");

        RuleFor(x => x.VideoUrl)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.VideoId))
            .WithMessage("VideoUrl is required");

        RuleFor(x => x.AudioId)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.AudioId))
            .WithMessage("AudioId cannot over limit 50 characters");

        RuleFor(x => x.AudioUrl)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.AudioId))
            .WithMessage("AudioUrl is required");
    }
}