using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Expense.UpdateSubExpense;

public class UpdateSubExpenseCommand : ICommand
{
    public UpdateSubExpenseCommand(Guid id, UpdateSubExpenseDto request)
        => (Id, Name, Price) = (id, request.Name, request.Price);

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public long Price { get; set; }
}
