using FluentValidation;

namespace EventHub.Application.Commands.Auth.SignOut;

public class SignOutCommandValidator : AbstractValidator<SignOutCommand>
{
    public SignOutCommandValidator()
    {
        // No validation rules needed since SignOutCommand has no properties to validate
    }
}
