using FluentValidation;

namespace EventHub.Application.Commands.Expense.DeleteExpense;

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Expense ID is required");
    }
}
