using FluentValidation;

namespace EventHub.Application.Commands.Payment.Checkout;

public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
{
    public CheckoutCommandValidator()
    {
        RuleFor(cmd => cmd.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .Length(2, 100).WithMessage("Customer name must be between 2 and 100 characters.");

        RuleFor(cmd => cmd.CustomerEmail)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(cmd => cmd.CustomerPhone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in valid international format.");

        RuleFor(cmd => cmd.EventId)
            .NotEmpty().WithMessage("Event ID is required.");

        RuleFor(cmd => cmd.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(cmd => cmd.TotalPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Total price cannot be negative.");

        RuleFor(cmd => cmd.CheckoutItems)
            .NotEmpty().WithMessage("At least one checkout item is required.");

        RuleForEach(cmd => cmd.CheckoutItems)
            .SetValidator(new CheckoutItemCommandValidator());
    }
}

public class CheckoutItemCommandValidator : AbstractValidator<CheckoutItemCommand>
{
    public CheckoutItemCommandValidator()
    {
        RuleFor(item => item.EventId)
            .NotEmpty().WithMessage("Event ID is required in checkout item.");

        RuleFor(item => item.TicketTypeId)
            .NotEmpty().WithMessage("Ticket type ID is required in checkout item.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1 in checkout item.");

        RuleFor(item => item.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Total price cannot be negative in checkout item.");
    }
}
