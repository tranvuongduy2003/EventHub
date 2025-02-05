using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Expense.DeleteSubExpense;

/// <summary>
/// Represents a command to delete a expense.
/// </summary>
/// <remarks>
/// This command is used to remove a expense by its unique identifier.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the expense to be deleted.
/// </summary>
/// <param name="Id">
/// A <see cref="Guid"/> representing the unique identifier of the expense.
/// </param>
public record DeleteSubExpenseCommand(Guid Id) : ICommand;
