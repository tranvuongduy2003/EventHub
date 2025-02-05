namespace EventHub.Application.SeedWork.DTOs.Expense;

public class UpdateExpenseDto
{
    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }
}
