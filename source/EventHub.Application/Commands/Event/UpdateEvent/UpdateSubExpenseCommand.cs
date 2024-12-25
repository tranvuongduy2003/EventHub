namespace EventHub.Application.Commands.Event.UpdateEvent;

public class UpdateSubExpenseCommand
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public long Price { get; set; }
}
