using EventHub.Domain.Shared.Enums.Event;
using FluentValidation;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.CoverImage)
            .NotNull().WithMessage("Cover image is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot over limit 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot over limit 1000 characters");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required")
            .LessThan(x => x.EndTime)
            .WithMessage("Start time must be less than end time");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be greater than start time");

        RuleFor(x => x.Categories)
            .NotNull().WithMessage("CategoryIds is required");
        RuleForEach(x => x.Categories)
            .NotEmpty().WithMessage("Category id cannot be empty")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid category id format");

        RuleFor(x => x.EventPaymentType)
            .NotNull().WithMessage("EEventPaymentType is required");

        RuleFor(x => x.TicketTypes)
            .NotNull()
            .When(x => x.EventPaymentType == EEventPaymentType.PAID)
            .WithMessage("TicketTypes is required");

        RuleFor(x => x.Reasons)
            .NotNull().WithMessage("Reasons is required");
        RuleForEach(x => x.Reasons)
            .NotNull().WithMessage("Reason is required")
            .MaximumLength(1000).WithMessage("Reason cannot over limit 1000 characters");

        RuleFor(x => x.EventSubImages)
            .NotNull().WithMessage("EventSubImages is required");

        RuleFor(x => x.IsPrivate)
            .NotNull().WithMessage("IsPrivate is required");

        RuleFor(x => x.EventCycleType)
            .NotNull().WithMessage("EEventCycleType is required");

        When(x => x.EmailContent != null, () =>
        {
            RuleFor(x => x.EmailContent!)
                .SetValidator(new CreateEmailContentCommandValidator());
        });
    }
}
