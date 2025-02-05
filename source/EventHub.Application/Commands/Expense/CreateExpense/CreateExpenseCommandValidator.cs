using FluentValidation;

namespace EventHub.Application.Commands.Expense.CreateExpense;

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Expense title is required");
    }
}
