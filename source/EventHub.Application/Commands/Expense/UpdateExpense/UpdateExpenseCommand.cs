using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Expense.UpdateExpense;

public class UpdateExpenseCommand : ICommand
{
    public UpdateExpenseCommand(Guid id, UpdateExpenseDto request)
        => (Id, Title, Total) = (id, request.Title, request.Total);

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }
}
