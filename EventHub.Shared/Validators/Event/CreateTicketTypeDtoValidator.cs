using EventHub.Shared.DTOs.Event;
using FluentValidation;

namespace EventHub.Shared.Validators.Event;

public class CreateTicketTypeDtoValidator : AbstractValidator<CreateTicketTypeDto>
{
    public CreateTicketTypeDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot over limit 100 characters");

        RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}