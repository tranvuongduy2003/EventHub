using FluentValidation;

namespace EventHub.Application.Commands.Expense.DeleteSubExpense;

public class DeleteSubExpenseCommandValidator : AbstractValidator<DeleteSubExpenseCommand>
{
    public DeleteSubExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("SubExpense ID is required");
    }
}
