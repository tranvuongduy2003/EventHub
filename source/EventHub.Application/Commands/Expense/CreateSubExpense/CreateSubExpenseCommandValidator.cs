using FluentValidation;

namespace EventHub.Application.Commands.Expense.CreateSubExpense;

public class CreateSubExpenseCommandValidator : AbstractValidator<CreateSubExpenseCommand>
{
    public CreateSubExpenseCommandValidator()
    {
        RuleFor(x => x.ExpenseId)
            .NotEmpty()
            .WithMessage("Expense ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("SubExpense name is required");
    }
}
