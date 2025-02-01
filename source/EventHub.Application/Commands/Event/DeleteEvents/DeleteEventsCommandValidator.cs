using FluentValidation;

namespace EventHub.Application.Commands.Event.DeleteEvents;

public class DeleteEventsCommandValidator : AbstractValidator<DeleteEventsCommand>
{
    public DeleteEventsCommandValidator()
    {
    }
}
