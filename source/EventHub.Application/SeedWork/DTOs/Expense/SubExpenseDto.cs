namespace EventHub.Application.SeedWork.DTOs.Expense;

public class SubExpenseDto
{
    public Guid Id { get; set; }

    public Guid ExpenseId { get; set; } = Guid.Empty;

    public string Name { get; set; } = string.Empty;

    public long Price { get; set; }
}
