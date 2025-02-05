using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Expense.CreateSubExpense;

/// <summary>
/// Represents a command to create a expense for an event.
/// </summary>
/// <remarks>
/// This command is used to create a new expense with the specified details for a particular event.
/// </remarks>
public class CreateSubExpenseCommand : ICommand<SubExpenseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSubExpenseCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new expense.
    /// </param>
    public CreateSubExpenseCommand(Guid expenseId, CreateSubExpenseDto request)
        => (ExpenseId, Name, Price) = (expenseId, request.Name, request.Price);

    public Guid ExpenseId { get; set; }

    public string Name { get; set; }

    public long Price { get; set; }
}
