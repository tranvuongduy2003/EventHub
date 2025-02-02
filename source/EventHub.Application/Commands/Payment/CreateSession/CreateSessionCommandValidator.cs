using FluentValidation;

namespace EventHub.Application.Commands.Payment.CreateSession;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
    }
}
