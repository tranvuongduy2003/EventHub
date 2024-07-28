using EventHub.Domain.DTOs.Ticket;
using FluentValidation;

namespace EventHub.Domain.Validators.Ticket;

public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
{
    public CreateTicketDtoValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer's name is required")
            .MaximumLength(100).WithMessage("Customer's name cannot over limit 100 characters");

        RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage("Customer's phone is required")
            .MaximumLength(100).WithMessage("Customer's phone cannot over limit 100 characters");

        RuleFor(x => x.CustomerEmail).NotEmpty().WithMessage("Customer's email is required")
            .MaximumLength(100).WithMessage("Customer's email cannot over limit 100 characters")
            .EmailAddress()
            .WithMessage("Customer's email must be an email");

        RuleFor(x => x.TicketTypeId).NotEmpty().WithMessage("Ticket type's id is required")
            .MaximumLength(50).WithMessage("Ticket type's id cannot over limit 50 characters");

        RuleFor(x => x.EventId).NotEmpty().WithMessage("Event's id is required")
            .MaximumLength(50).WithMessage("Event's id cannot over limit 50 characters");

        RuleFor(x => x.PaymentId).NotEmpty().WithMessage("Payment's id is required")
            .MaximumLength(50).WithMessage("Payment's id cannot over limit 50 characters");

        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
    }
}