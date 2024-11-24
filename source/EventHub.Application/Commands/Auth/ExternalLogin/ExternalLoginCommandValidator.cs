using FluentValidation;

namespace EventHub.Application.Commands.Auth.ExternalLogin;

internal sealed class ExternalLoginCommandValidator : AbstractValidator<ExternalLoginCommand>
{
    public ExternalLoginCommandValidator()
    {
        RuleFor(x => x.Provider).NotEmpty();
        RuleFor(x => x.ReturnUrl).NotEmpty();
    }
}