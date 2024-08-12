using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Enums.Event;
using FluentValidation;

namespace EventHub.Shared.Validators.Event;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MaximumLength(50).WithMessage("AuthorId cannot over limit 50 characters");

        RuleFor(x => x.CoverImage).NotNull().WithMessage("Cover image is required");

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot over limit 100 characters");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot over limit 1000 characters");

        RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required");

        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required")
            .LessThan(x => x.EndTime)
            .When(x => x.EndTime != null)
            .WithMessage("Start time must be less than end time");
        ;

        RuleFor(x => x.EndTime).NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime)
            .When(x => x.StartTime != null)
            .WithMessage("End time must be greater than start time");

        RuleFor(x => x.Promotion)
            .InclusiveBetween(0.0, 1.0)
            .When(x => x.EmailContent != null)
            .WithMessage("Promotion must be in range from 0.0 to 1.0");

        RuleFor(x => x.CategoryIds).NotNull().WithMessage("CategoryIds is required");
        RuleForEach(x => x.CategoryIds).NotNull().WithMessage("Category's id is required")
            .MaximumLength(50).WithMessage("Category's id cannot over limit 50 characters");

        RuleFor(x => x.EventPaymentType).NotNull().WithMessage("EEventPaymentType is required");

        RuleFor(x => x.TicketTypes)
            .NotNull()
            .When(x => x.EventPaymentType == EEventPaymentType.PAID)
            .WithMessage("TicketTypes is required");
        //RuleForEach(x => x.TicketTypes.Select(type => JsonConvert.DeserializeObject<TicketTypeCreateRequest>(type))).NotNull().WithMessage("Ticket Type is required")
        //    .SetValidator(new CreateTicketTypeDtoValidator());

        RuleFor(x => x.Reasons).NotNull().WithMessage("Reasons is required");
        RuleForEach(x => x.Reasons).NotNull().WithMessage("Reason is required")
            .MaximumLength(1000).WithMessage("Reason cannot over limit 1000 characters");

        RuleFor(x => x.EventSubImages).NotNull().WithMessage("EventSubImages is required");

        RuleFor(x => x.IsPrivate).NotNull().WithMessage("IsPrivate is required");

        RuleFor(x => x.EventCycleType).NotNull().WithMessage("EEventCycleType is required");

        RuleFor(x => x.EmailContent).SetValidator(new CreateEmailContentDtoValidator())
            .When(x => x.EmailContent != null);
    }
}