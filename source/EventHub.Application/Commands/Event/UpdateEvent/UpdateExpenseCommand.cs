using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.UpdateEvent;

public class UpdateExpenseCommand : ICommand
{
    public UpdateExpenseCommand()
    {
    }

    public Guid? Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }

    public List<UpdateSubExpenseCommand> SubExpenses { get; set; } = new List<UpdateSubExpenseCommand>();
}
