using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateExpenseCommand : ICommand
{
    public CreateExpenseCommand()
    {
    }

    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }

    public List<CreateSubExpenseCommand> SubExpenses { get; set; } = new List<CreateSubExpenseCommand>();
}
