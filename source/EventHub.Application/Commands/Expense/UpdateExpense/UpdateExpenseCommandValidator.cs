using FluentValidation;

namespace EventHub.Application.Commands.Expense.UpdateExpense;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Expense ID is required");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Expense title is required");
    }
}
