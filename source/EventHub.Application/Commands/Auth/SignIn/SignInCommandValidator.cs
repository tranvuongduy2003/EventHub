using FluentValidation;

namespace EventHub.Application.Commands.Auth.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
    }
}
