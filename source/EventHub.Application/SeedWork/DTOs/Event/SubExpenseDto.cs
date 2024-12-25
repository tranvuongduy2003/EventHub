namespace EventHub.Application.SeedWork.DTOs.Event;

public class SubExpenseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public long Price { get; set; }
}
