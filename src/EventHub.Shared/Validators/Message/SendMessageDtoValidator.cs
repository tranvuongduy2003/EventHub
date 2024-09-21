using EventHub.Shared.DTOs.Message;
using FluentValidation;

namespace EventHub.Shared.Validators.Message;

public class SendMessageDtoValidator : AbstractValidator<SendMessageDto>
{
    public SendMessageDtoValidator()
    {
        RuleFor(x => x.ConversationId.ToString())
            .NotEmpty().WithMessage("ConversationId is required")
            .MaximumLength(50).WithMessage("ConversationId cannot over limit 50 characters");

        RuleFor(x => x.AuthorId.ToString())
            .NotEmpty().WithMessage("AuthorId is required")
            .MaximumLength(50).WithMessage("AuthorId cannot over limit 50 characters");

        RuleFor(x => x.Content)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Content))
            .WithMessage("Content cannot over limit 1000 characters");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .WithMessage("ImageUrl cannot over limit 255 characters");

        RuleFor(x => x.ImageFileName)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.ImageFileName))
            .WithMessage("ImageFileName cannot over limit 255 characters");

        RuleFor(x => x.VideoUrl)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.VideoUrl))
            .WithMessage("VideoUrl cannot over limit 255 characters");

        RuleFor(x => x.VideoFileName)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.VideoFileName))
            .WithMessage("VideoFileName cannot over limit 255 characters");

        RuleFor(x => x.AudioUrl)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.AudioUrl))
            .WithMessage("AudioUrl cannot over limit 255 characters");

        RuleFor(x => x.AudioFileName)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.AudioFileName))
            .WithMessage("AudioFileName cannot over limit 255 characters");
    }
}