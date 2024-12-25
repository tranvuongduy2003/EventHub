namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateSubExpenseCommand
{
    public string Name { get; set; } = string.Empty;

    public long Price { get; set; }
}
