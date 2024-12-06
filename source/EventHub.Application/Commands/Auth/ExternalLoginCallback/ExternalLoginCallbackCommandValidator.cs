using FluentValidation;

namespace EventHub.Application.Commands.Auth.ExternalLoginCallback;

internal sealed class ExternalLoginCallbackCommandValidator : AbstractValidator<ExternalLoginCallbackCommand>
{
    public ExternalLoginCallbackCommandValidator()
    {
        RuleFor(x => x.ReturnUrl).NotEmpty();
    }
}

