using FluentValidation;

namespace EventHub.Application.Commands.Expense.UpdateSubExpense;

public class UpdateSubExpenseCommandValidator : AbstractValidator<UpdateSubExpenseCommand>
{
    public UpdateSubExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("SubExpense ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("SubExpense title is required");
    }
}
