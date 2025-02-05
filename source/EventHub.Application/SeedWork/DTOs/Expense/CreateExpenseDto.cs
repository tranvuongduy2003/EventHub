namespace EventHub.Application.SeedWork.DTOs.Expense;

public class CreateExpenseDto
{
    public Guid EventId { get; set; } = Guid.Empty;

    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }
}
