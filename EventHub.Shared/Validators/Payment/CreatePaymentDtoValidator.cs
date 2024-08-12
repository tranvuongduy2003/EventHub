using EventHub.Shared.DTOs.Payment;
using FluentValidation;

namespace EventHub.Shared.Validators.Payment;

public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentDtoValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("Event's id is required")
            .MaximumLength(50).WithMessage("Event's id cannot over limit 50 characters");

        RuleFor(x => x.TicketQuantity).NotEmpty().WithMessage("Ticket quantity is required")
            .GreaterThan(0)
            .WithMessage("Ticket quantity must be greater than 0");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User's id is required")
            .MaximumLength(50).WithMessage("User's id cannot over limit 50 characters");

        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer's name is required")
            .MaximumLength(100).WithMessage("Customer's name cannot over limit 100 characters");

        RuleFor(x => x.CustomerPhone).NotEmpty().WithMessage("Customer's phone is required")
            .MaximumLength(100).WithMessage("Customer's phone cannot over limit 100 characters");

        RuleFor(x => x.CustomerEmail).NotEmpty().WithMessage("Customer's email is required")
            .MaximumLength(100).WithMessage("Customer's email cannot over limit 100 characters")
            .EmailAddress()
            .WithMessage("Customer's email must be an email");

        RuleFor(x => x.TotalPrice).NotEmpty().WithMessage("Total price is required")
            .GreaterThan(0)
            .WithMessage("Total price must be greater than 0");

        RuleFor(x => x.Discount).NotEmpty().WithMessage("Discount is required")
            .InclusiveBetween(0.0, 1.0)
            .WithMessage("Discount must be in range from 0.0 to 1.0");

        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");

        RuleFor(x => x.UserPaymentMethodId).NotEmpty().WithMessage("UserPaymentMethodId is required")
            .MaximumLength(50).WithMessage("UserPaymentMethodId cannot over limit 50 characters");

        RuleFor(x => x.PaymentSessionId).NotEmpty().WithMessage("PaymentSessionId is required")
            .MaximumLength(50).WithMessage("PaymentSessionId cannot over limit 50 characters");
    }
}