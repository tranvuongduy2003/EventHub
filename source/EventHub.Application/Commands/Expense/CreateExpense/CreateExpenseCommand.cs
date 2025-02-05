using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Expense.CreateExpense;

/// <summary>
/// Represents a command to create a expense for an event.
/// </summary>
/// <remarks>
/// This command is used to create a new expense with the specified details for a particular event.
/// </remarks>
public class CreateExpenseCommand : ICommand<ExpenseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateExpenseCommand"/> class.
    /// </summary>
    /// <param name="request">
    /// The data transfer object containing the details for the new expense.
    /// </param>
    public CreateExpenseCommand(CreateExpenseDto request)
        => (EventId, Title, Total) = (request.EventId, request.Title, request.Total);

    public Guid EventId { get; set; }

    public string Title { get; set; }

    public long Total { get; set; }
}
